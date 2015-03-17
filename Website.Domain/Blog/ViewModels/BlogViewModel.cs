using MLWD.Umbraco.Collections;
using Website.Domain.Blog.DocTypes;

namespace Website.Domain.Blog.ViewModels
{
    public class BlogViewModel
    {
        public BlogListing Content { get; set; }

        public IPagedList<BlogDetails> Blogs { get; set; }
    }
}