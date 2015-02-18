using Website.Domain.Blog.DocTypes;
using Yomego.CMS.Core.Collections;

namespace Website.Domain.Blog.ViewModels
{
    public class BlogViewModel
    {
        public BlogListing Content { get; set; }

        public IPagedList<BlogDetails> Blogs { get; set; }
    }
}