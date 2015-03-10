using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BoboBrowse.Api;
using BoboBrowse.Facets;
using BoboBrowse.Facets.impl;
using C5;
using Examine;
using Examine.LuceneEngine.Config;
using Examine.LuceneEngine.SearchCriteria;
using Examine.Providers;
using Examine.SearchCriteria;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Collections;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Mvc.Models.Interfaces;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Core.Umbraco.Search;
using Yomego.CMS.Core.Utils;

namespace Yomego.CMS.Umbraco.Services
{
    public delegate void AfterModelBoundEventHandler(Content content);

    public abstract class ContentService : BaseService
    {
        public event AfterModelBoundEventHandler AfterModelBound;

        protected void OnAfterModelBound(Content content)
        {
            if (AfterModelBound != null)
            {
                AfterModelBound(content);
            }
        }


        public int Save(Content content)
        {
            return Save(content, true);
        }

        #region Abstract Methods

        public abstract int Save(Content content, bool publish);

        public abstract Content Get(string url);

        public abstract Content Get(int id);

        public abstract T Get<T>(int id) where T : Content, new();

        public abstract T GetRoot<T>() where T : Content, new();

        public abstract string GetCulture(int id);

        public abstract System.Collections.Generic.IList<ISitemapItem> GetSitemapPages();

        #endregion

        public System.Collections.Generic.IList<Content> GetChildren(int parentId)
        {
            return Get<Content>(Criteria.WithParentId(parentId)
                                        .OrderByAscending(SearchOrder.ManualSort)
                                        .AndCulture(null)); // Don't filter on culture
        }

        public virtual IPagedList<T> Get<T>() where T : Content, new()
        {
            return Get<T>(Criteria.All());
        }

        public virtual System.Collections.Generic.IList<int> GetIds<T>(ICriteria criteria)
        {
            var searchResults = DoSearch<T>(criteria);

            return searchResults.Select(r => r.Id).ToList();
        }

        private ISearchResults DoSearch<T>(ICriteria criteria)
        {
            Type type = typeof(T);

            // Ensure the Type is the only type in the TypesToSearch collection
            if (criteria.TypesToSearch == null)
                criteria.TypesToSearch = new List<Type>();

            if(criteria.TypesToSearch.Count == 0)
                criteria.TypesToSearch.Add(type);
           
            var provider = ExamineManager.Instance.SearchProviderCollection[Yomego.CMS.Constants.Examine.MainExamineSearchProvider];

            var searchCriteria = BuildSearchCriteria(provider, criteria);

            var searchResults = provider.Search(searchCriteria);

            return searchResults;
        }

        public System.Collections.Generic.IList<Facet> FacetSearch(ICriteria criteria)
        {
            var provider = ExamineManager.Instance.SearchProviderCollection[Yomego.CMS.Constants.Examine.MainExamineSearchProvider];

            var rawQuery = BuildSearchCriteria(provider, criteria).ToString();

            var facets = GenerateFacets(Yomego.CMS.Constants.Examine.MainExamineIndexset, rawQuery, criteria.FacetField);

            return facets;
        }

        public virtual IPagedList<T> Get<T>(ICriteria criteria) where T : Content, new()
        {
            var searchResults = DoSearch<T>(criteria);

            var paged = searchResults.Skip(criteria.Page * criteria.PageSize).Take(criteria.PageSize);

            var results = paged.Select(r => Get<T>(r.Id));

            return results.Where(i => i != null).ToPagedList(criteria.Page, criteria.PageSize, searchResults.Count());
        }

        protected string GetTypeStringForIndex(Type type)
        {
            var attr = GetAttribute<ContentTypeAttribute>(type);

            if (attr == null || String.IsNullOrWhiteSpace(attr.Alias))
            {
                // Default to type name
                return type.Name;
            }
            else
            {
                return attr.Alias;
            }
        }


        private T GetAttribute<T>(Type type)
        {
            T retVal;

            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0)
            {
                retVal = (T)attributes[0];
            }
            else
            {
                retVal = default(T);
            }

            return retVal;
        }

        public virtual string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        public virtual T First<T>() where T : Content, new()
        {
            return First<T>(Criteria.All());
        }

        public virtual T First<T>(ICriteria criteria) where T : Content, new()
        {
            var searchResults = DoSearch<T>(criteria);
            
            var result = searchResults.FirstOrDefault();

            if (result == null)
            {
                return default(T);
            }

            return Get<T>(result.Id);
        }


        private ISearchCriteria BuildSearchCriteria(BaseSearchProvider provider, ICriteria search)
        {
            if (search == null)
                search = new Criteria();

            var searchCriteria = provider.CreateSearchCriteria(BooleanOperation.And);

            // The constructor is internal so having to use reflection here. Not ideal, there must be a better way of doing it.

            var query = (IBooleanOperation) typeof(LuceneBooleanOperation).GetConstructor(
                  BindingFlags.NonPublic | BindingFlags.Instance,
                  null, new Type[] { typeof(LuceneSearchCriteria) }, null).Invoke(new object[] { (LuceneSearchCriteria)searchCriteria });

            // [ML] - This is used for main site searching

            if (search.TypesAsStringToSearch == null)
            {
                if (search.TypesToSearch != null)
                {
                    search.TypesAsStringToSearch = new List<string>();

                    // If type equals Node then we don't want to filter, we'll return all Node Types instead.

                    if (search.TypesToSearch.Count == 1 && search.TypesToSearch[0] == typeof (Content))
                    {
                        search.TypesAsStringToSearch.Add("content"); // This will return ALL nodes
                    }
                    else
                    {
                        search.TypesAsStringToSearch.AddRange(search.TypesToSearch.Select(GetTypeStringForIndex));
                    }
                }
            }

            if (search.TypesAsStringToSearch != null)
            {
                // If type equals Node then we don't want to filter, we'll return all Node Types instead.
                if (search.TypesAsStringToSearch.Count == 1 && search.TypesAsStringToSearch[0].ToLower() == "content")
                {
                    query = query.And().Field("__IndexType", "content"); // This will return ALL nodes
                }
                else
                {
                    var types = search.TypesAsStringToSearch.Select(t => t.ToLower()).ToArray();

                    query = query.And().GroupedOr(new []{"__NodeTypeAlias"}, types);
                }
            }

            if (search.SearchItems != null)
            {
                if (search.SearchItems.Item1.Count == 1 && search.SearchItems.Item2.Count == 1 && search.SearchItems.Item3 == OperatorEnum.OR)
                {
                    // If we're search one value against one search key then we can just use Field()
                    query = query.And().Field(search.SearchItems.Item1.First(), search.SearchItems.Item2.First());
                }
                else
                {
                    switch (search.SearchItems.Item3)
                    {
                        case OperatorEnum.AND:
                            query = search.SearchItems.Item2.Aggregate(query, (current, field) => current.And().GroupedAnd(search.SearchItems.Item1, field));
                            break;
                        case OperatorEnum.NOT:
                            query = query.And().GroupedNot(search.SearchItems.Item1.ToArray(), search.SearchItems.Item2.ToArray());
                            break;
                        default:
                            query = query.And().GroupedOr(search.SearchItems.Item1.ToArray(), search.SearchItems.Item2.ToArray());
                            break;
                    }
                }
            }

            string orderColumn = search.CustomOrder;

            if (string.IsNullOrWhiteSpace(orderColumn))
            {
                switch (search.Order)
                {
                    case SearchOrder.RankDesc:
                        // LEAVE FOR RANK
                        break;
                    case SearchOrder.PublishDate:
                        orderColumn = "SystemPublishDate";
                        break;
                    case SearchOrder.DateCreated:
                    case SearchOrder.Unknown:
                        orderColumn = "createDate";
                        break;
                    case SearchOrder.ManualSort:
                        orderColumn = "SystemSortOrder";
                        break;
                }
            }

            if (orderColumn != null)
            {
                if(search.OrderAscending == true)
                    query = query.And().OrderBy(new []{orderColumn});
                else
                    query = query.And().OrderByDescending(orderColumn);
            }

            return query.Compile();
        }

        public T Parent<T>(Content content)
        {
            var contentItem = First<Content>(Criteria.WithParentsOf(content)
                                                     .AndTypes(new [] { typeof(T) })
                                                     .OrderByAscending(SearchOrder.ManualSort)
                                                     .AndCulture(null)); // Don't filter on culture

            if (contentItem == null)
            {
                return default(T);
            }

            var convertedModel = Convert.ChangeType(contentItem, typeof(T));

            return (T)convertedModel;
        }

        #region Bobo Browser

        private System.Collections.Generic.IList<Facet> GenerateFacets(string indexSetName, string luceneQuery, string facetField)
        {
            var indexPath = GetIndexPath(indexSetName);

            var fields = new[] { facetField };

            var facetGen = new FacetGenerator(indexPath, luceneQuery, fields, 1, "contents", "directoryType");

            var facetList = new List<Facet>();

            var facets = facetGen.GenerateFacets();

            if (facets != null)
            {
                foreach (var facetKey in facets.Keys)
                {
                    facetList.AddRange(BuildFacet(facets[facetKey]));
                }
            }

            return facetList;
        }


        private IEnumerable<Facet> BuildFacet(IEnumerable<BrowseFacet> browseFacets)
        {
            return browseFacets.Select(browseFacet => new Facet()
                {
                    Name = browseFacet.Value.ToString(),
                    Count = browseFacet.HitCount
                });
        }

        private string GetIndexPath(string indexSetName)
        {
            IndexSetCollection sets = Examine.LuceneEngine.Config.IndexSets.Instance.Sets;
            IndexSet set = sets[indexSetName];
            System.IO.DirectoryInfo dir = set.IndexDirectory;
            string path = System.IO.Path.Combine(dir.FullName, "Index");
            return path;
        }

        #endregion
    }
}
