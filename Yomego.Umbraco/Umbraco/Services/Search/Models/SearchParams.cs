using System;
using Yomego.Umbraco.Umbraco.Services.Search.Enums;

namespace Yomego.Umbraco.Umbraco.Services.Search.Models
{
    public class SearchParams
    {
        public SearchParams()
        {
            
        }

        public SearchParams(string keywords = null, string types = null, int? parentId = null, string culture = null, string custom = null, string order = null, bool @descending = false, string categories = null)
        {
            Keywords = keywords;
            Types = types;
            ParentId = parentId;
            Culture = culture;
            Custom = custom;
            Order = order;
            Descending = @descending;
            Categories = categories;
        }

        public string Keywords { get;set; }

        public string Types { get; set; }

        public int? ParentId { get; set; }

        public string Culture { get; set; }

        public string Custom { get; set; }

        public bool HasOrder
        {
            get { return !string.IsNullOrWhiteSpace(Order); }
        }

        public string Order { get; set; }

        public SearchOrder OrderEnum
        {
            get
            {
                var searchOrder = SearchOrder.ManualSort;

                if (!string.IsNullOrWhiteSpace(Order))
                {
                    searchOrder = (SearchOrder)Enum.Parse(typeof(SearchOrder), Order, true);
                }

                return searchOrder;
            }
        }

        public bool Descending { get; set; }

        public string Categories { get; set; }

        public string Tags { get; set; }
        
        public int Page { get; set; }
        
        public int PageSize { get; set; }
    }
}