using System;
using Yomego.CMS.Core.Umbraco.Search;

namespace Yomego.CMS.Core.Umbraco.Model
{
    public class SearchParams
    {
        public SearchParams()
        {
            
        }

        public SearchParams(string keywords = null, string types = null, int? parentId = null, string culture = null, string custom = null, string order = null, bool @descending = false)
        {
            Keywords = keywords;
            Types = types;
            ParentId = parentId;
            Culture = culture;
            Custom = custom;
            Order = order;
            Descending = @descending;
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
    }
}