using Website.Domain.Blog.DocTypes;
using Website.Domain.Home.DocTypes;
using Yomego.CMS.Core.Collections;

namespace Website.Domain.Home.ViewModels
{
    public class HomeViewModel
    {
        public Homepage Content { get; set; }

        public IPagedList<BlogDetails> Blogs { get; set; }

        public IPagedList<BlogDetails> Work { get; set; }
    }
}