using System.Collections.Generic;
using System.Web;
using MLWD.Umbraco.Umbraco.Services.Search.Models;
using Website.Domain.Blog.DocTypes;
using System.Linq;

namespace Website.Domain.Blog.ViewModels
{
    public class BlogListingViewModel
    {
        public BlogListingViewModel()
        {
            Keywords = new Dictionary<string, string>();
        }

        public BlogListing Listing { get; set; }

        public IList<Facet> Categories { get; set; }

        public IList<Facet> Tags { get; set; }

        public Dictionary<string, string> Keywords { get; set; } 

        public IHtmlString FormattedString(int resultCount)
        {
            if (!Keywords.Any())
            {
                return new HtmlString(string.Empty);    
            }

            var joined = Keywords.Select(i => string.Format("{0} - '{1}'", i.Key, i.Value)).ToArray();

            return new HtmlString(string.Format("<p>Found {0} results using the search criteria {1}.</p>", resultCount, string.Join(", ", joined)));
        }

    }
}