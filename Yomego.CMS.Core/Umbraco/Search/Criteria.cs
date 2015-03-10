using System;
using Yomego.CMS.Core.Utils;

namespace Yomego.CMS.Core.Umbraco.Search
{
    public class Criteria : TypedCriteria<Criteria>
    {
        private static Criteria New()
        {
            return new Criteria();
        }

        public static Criteria WithKeywords(string keywords)
        {
            return New().AndKeywords(keywords);
        }

        public Criteria AndKeywords(string keywords)
        {
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var searchTerms = StringUtils.CleanTextForLucene(keywords.ToLower());
                var searchTermsArray = searchTerms.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                AddSearchItem(new[] { "body", "title", "summary", "content" }, searchTermsArray);
            }

            return this;
        }

        public static Criteria WithName(string name)
        {
            return New().AndName(name);
        }

        public Criteria AndName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                AddSearchItem("nodeName", name);
            }

            return this;
        }

        public static Criteria WithFacetField(string field)
        {
            return New().AndFacetField(field);
        }

        public Criteria AndFacetField(string field)
        {
            if (!string.IsNullOrWhiteSpace(field))
            {
                FacetField = field;
            }

            return this;
        }
    }
}
