using MLWD.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Blog.DocTypes
{
    [UmbracoRoute("Blog")]
    public class BlogListing : Page
    {
        public BlogListing(IPublishedContent content) : base(content)
        {
        }

        public int BlogPageCount { get; set; }
    }
}
