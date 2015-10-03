using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using BoboBrowse.Api;
using Examine;
using Examine.LuceneEngine.Config;
using Examine.LuceneEngine.SearchCriteria;
using Examine.Providers;
using Examine.SearchCriteria;
using umbraco.MacroEngines;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Yomego.Umbraco.Collections;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Umbraco.Services.Content.Facets;
using Yomego.Umbraco.Umbraco.Services.Search.Enums;
using Yomego.Umbraco.Umbraco.Services.Search.Models;
using Yomego.Umbraco.Umbraco.Services.Search.Models.Interfaces;
using Yomego.Umbraco.Utils;

namespace Yomego.Umbraco.Umbraco.Services.Content
{
    public delegate void AfterModelBoundEventHandler(PublishedContentModel content);

    public abstract class ContentService : BaseService<App>
    {
        public event AfterModelBoundEventHandler AfterModelBound;

        protected void OnAfterModelBound(PublishedContentModel content)
        {
            if (AfterModelBound != null)
            {
                AfterModelBound(content);
            }
        }

        #region Abstract Methods

        public abstract void ClearCache();

        public abstract object Get(string url);

        public abstract object Get(int id);

        public abstract object Get(IPublishedContent model);

        public abstract T Get<T>(int id) where T : PublishedContentModel;

        public abstract T GetRoot<T>() where T : PublishedContentModel;

        public abstract string GetCulture(int id);

        public abstract void Save(PublishedContentModel content, bool publish = true, int? parentId = null);

        public abstract DynamicNode RootNode { get; }

        #endregion


        public IList<PublishedContentModel> GetChildren(int parentId)
        {
            var criteria = Criteria.WithParentId(parentId).OrderByAscending(SearchOrder.ManualSort).AndCulture(null);

            return Get<PublishedContentModel>(); 
        }

        public virtual IPagedList<T> Get<T>() where T : PublishedContentModel
        {
            return Get<T>(Criteria.All());
        }

        public virtual IPagedList<T> Get<T>(ICriteria criteria) where T : PublishedContentModel
        {
            var searchResults = DoSearch<T>(criteria);

            var paged = searchResults.Skip(criteria.Page * criteria.PageSize).Take(criteria.PageSize);

            var results = paged.Select(r => (T)Get(r.Id));

            return results.Where(i => i != null).ToPagedList(criteria.Page, criteria.PageSize, searchResults.Count());
        }

        public virtual T First<T>() where T : PublishedContentModel
        {
            return First<T>(Criteria.All());
        }

        public virtual T First<T>(ICriteria criteria) where T : PublishedContentModel
        {
            var searchResults = DoSearch<T>(criteria);

            var result = searchResults.FirstOrDefault();

            if (result == null)
            {
                return default(T);
            }

            return Get<T>(result.Id);
        }

        public virtual IList<int> GetIds<T>(ICriteria criteria)
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

            if (criteria.TypesToSearch.Count == 0 && typeof(object) != type)
                criteria.TypesToSearch.Add(type);
           
            var provider = ExamineManager.Instance.SearchProviderCollection[Constants.Examine.MainExamineSearchProvider];

            var searchCriteria = BuildSearchCriteria(provider, criteria);

            var searchResults = provider.Search(searchCriteria);

            return searchResults;
        }

        public IList<Facet> FacetSearch(ICriteria criteria)
        {
            var provider = ExamineManager.Instance.SearchProviderCollection[Constants.Examine.MainExamineSearchProvider];

            var rawQuery = BuildSearchCriteria(provider, criteria).ToString();

            var facets = GenerateFacets(Constants.Examine.MainExamineIndexset, rawQuery, criteria.FacetField);

            return facets;
        }

        private string GetNameForType(Type type)
        {
            var routeAttribute = type.GetCustomAttributes(typeof(UmbracoRouteAttribute), true).FirstOrDefault() as UmbracoRouteAttribute;

            if (routeAttribute != null && !string.IsNullOrWhiteSpace(routeAttribute.Alias))
            {
                return routeAttribute.Alias;
            }

            return type.Name;
        }

        public virtual string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.Name;
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

                    if (search.TypesToSearch.Count == 1 && search.TypesToSearch[0] == typeof (PublishedContentModel))
                    {
                        search.TypesAsStringToSearch.Add("content"); // This will return ALL nodes
                    }
                    else
                    {
                        search.TypesAsStringToSearch.AddRange(search.TypesToSearch.Select(GetNameForType));
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
                foreach (var searchItem in search.SearchItems)
                {
                    var keys = searchItem.Item1.ToArray();
                    var values = searchItem.Item2.Select(i => i).ToArray();

                    switch (searchItem.Item3)
                    {
                        case OperatorEnum.AND:
                            query = query.And().GroupedAnd(keys, values);
                            break;
                        case OperatorEnum.NOT:
                            query = query.And().GroupedNot(keys, values);
                            break;
                        default:
                            query = query.And().GroupedOr(keys, values);
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
                query = search.OrderAscending == true ? query.And().OrderBy(new []{orderColumn}) : query.And().OrderByDescending(orderColumn);
            }

            return query.Compile();
        }

        #region Faceted

        private IList<Facet> GenerateFacets(string indexSetName, string luceneQuery, string facetField)
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
                    Name = browseFacet.Value.ToString().FixUnTokenized(),
                    Count = browseFacet.HitCount
                });
        }

        private string GetIndexPath(string indexSetName)
        {
            IndexSetCollection sets = IndexSets.Instance.Sets;
            IndexSet set = sets[indexSetName];
            DirectoryInfo dir = set.IndexDirectory;
            string path = Path.Combine(dir.FullName, "Index");
            return path;
        }

        #endregion
    }
}
