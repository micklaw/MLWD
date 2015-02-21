using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yomego.CMS.Core.Umbraco.Search;

namespace Website.Domain.Shared.Search
{
    public class SearchCriteria : Criteria
    {
        private static SearchCriteria Init()
        {
            return new SearchCriteria();
        }

        public static SearchCriteria WithBlogCategory(string category)
        {
            return Init().AndBlogCategory(category);
        }

        public SearchCriteria AndBlogCategory(string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                AddSearchItem("blogCategory", category);
            }

            return this;
        }

        public static SearchCriteria WithBlogTag(string category)
        {
            return Init().AndBlogTag(category);
        }

        public SearchCriteria AndBlogTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                AddSearchItem("blogTags", tag);
            }

            return this;
        }

        public static SearchCriteria WithBlogKeyword(string category)
        {
            return Init().AndBlogKeyword(category);
        }

        public SearchCriteria AndBlogKeyword(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                AddSearchItem(new [] {"blogCategory","blogTags","blogTitle","blogDescription","blogSummary"}, new[]{keyword});
            }

            return this;
        }
    }
}
