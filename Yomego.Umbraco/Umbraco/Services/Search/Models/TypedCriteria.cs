using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Models.PublishedContent;
using Yomego.Umbraco.Umbraco.Services.Search.Enums;
using Yomego.Umbraco.Umbraco.Services.Search.Models.Interfaces;

namespace Yomego.Umbraco.Umbraco.Services.Search.Models
{
    public abstract class TypedCriteria<T> : ICriteria where T : TypedCriteria<T>
    {
        private List<Tuple<List<string>, List<string>, OperatorEnum>> _SearchItems { get; set; }

        // Explicit implementation is deliberate here as not to expose SearchItems to inheriters,
        // they should use the AddSearchItem methods instead
        List<Tuple<List<string>, List<string>, OperatorEnum>> ICriteria.SearchItems
        {
            get
            {
                return _SearchItems;
            }
            set
            {
                _SearchItems = value;
            }
        }

        public void AddSearchItem(string key, string value, OperatorEnum operatorType = OperatorEnum.OR)
        {
            AddSearchItem(new[] { key }, new[] { (value ?? string.Empty).ToLower() }, operatorType);
        }

        public void AddSearchItem(IEnumerable<string> keys, IEnumerable<string> values, OperatorEnum operatorType = OperatorEnum.OR)
        {
            var items = (this as ICriteria).SearchItems;

            if (items == null)
            {
                items = new List<Tuple<List<string>, List<string>, OperatorEnum>>();
            }

            items.Add(new Tuple<List<string>, List<string>, OperatorEnum>(keys.ToList(), values.Select(i => (i ?? string.Empty).ToLower()).ToList(), operatorType));

            (this as ICriteria).SearchItems = items;
        }

        public TypedCriteria()
        {
            Page = 0; // Default to first page
            PageSize = 10;
            TypesToSearch = new List<Type>();
        }

        private static TypedCriteria<T> New()
        {
            return Activator.CreateInstance(typeof(T)) as T;
        }

        public static TypedCriteria<T> All()
        {
            return New().AndPaging(0, 100000);
        }

        public TypedCriteria<T> OrderBy(string field, bool descending = false)
        {
            CustomOrder = field;
            OrderAscending = !descending;
            return this;
        }

        public TypedCriteria<T> OrderByDescending(SearchOrder order)
        {
            Order = order;
            OrderAscending = false;
            return this;
        }

        public TypedCriteria<T> OrderByAscending(SearchOrder order)
        {
            Order = order;
            OrderAscending = true;
            return this;
        }

        public static TypedCriteria<T> WithParentsOf(PublishedContentModel content)
        {
            return New().AndParentsOf(content);
        }

        public TypedCriteria<T> AndParentsOf(PublishedContentModel content)
        {
            if (content != null && !string.IsNullOrWhiteSpace(content.Path))
            {
                var ids = content.Path.Replace("-1", string.Empty).Trim(',').Split(',');

                if (ids.Length > 0)
                {
                    AddSearchItem(new[] { "id" }, ids);
                }
            }

            return this;
        }

        public static TypedCriteria<T> WithIsActive(bool active)
        {
            return New().AndIsActive(active);
        }

        public TypedCriteria<T> AndIsActive(bool active)
        {
            AddSearchItem("isActive", active ? "1" : "0");
            return this;
        }

        public static TypedCriteria<T> WithCriteria(string key, string value)
        {
            return New().AndCriteria(key, value);
        }

        public TypedCriteria<T> AndCriteria(string key, string value)
        {
            AddSearchItem(key, value);
            return this;
        }

        public static TypedCriteria<T> WithPaging(int page, int pageSize)
        {
            return New().AndPaging(page, pageSize);
        }

        public static TypedCriteria<T> WithParentId(int id)
        {
            return New().AndParentId(id);
        }

        public TypedCriteria<T> AndParentId(int id)
        {
            AddSearchItem("SystemParentId", id.ToString());
            return this;
        }

        public TypedCriteria<T> AndPaging(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
            return this;
        }

        public static TypedCriteria<T> WithCulture(string culture)
        {
            return New().AndCulture(culture);
        }

        public TypedCriteria<T> AndCulture(string culture)
        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                AddSearchItem("SystemCulture", culture.Replace("-", "").ToLower());
            }

            return this;
        }

        public static TypedCriteria<T> WithTypes(params Type[] types)
        {
            return New().AndTypes(types);
        }

        public TypedCriteria<T> AndTypes(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (!TypesToSearch.Contains(type))
                    {
                        TypesToSearch.Add(type);
                    }
                }
            }

            return this;
        }


        public TypedCriteria<T> AndTypes(params string[] types)
        {
            if (types != null && types.Length > 0)
            {
                TypesAsStringToSearch = new List<string>();

                foreach (var type in types)
                {
                    if (!TypesAsStringToSearch.Contains(type))
                    {
                        TypesAsStringToSearch.Add(type);
                    }
                }
            }

            return this;
        }

        public string CustomOrder { get; set; }

        public string FacetField { get; set; }

        public SearchOrder Order { get; set; }

        public IList<Type> TypesToSearch { get; set; }

        public List<string> TypesAsStringToSearch { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public bool? OrderAscending { get; private set; }
    }
}
