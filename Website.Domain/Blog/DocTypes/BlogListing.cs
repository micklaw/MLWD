using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using Umbraco.Core.Models;

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
