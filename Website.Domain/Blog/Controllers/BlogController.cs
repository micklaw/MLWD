using System.Web.Mvc;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Blog.ViewModels;
using Yomego.CMS.Core.Umbraco.Search;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Blog.Controllers
{
    public class BlogController : BaseBlogController
    {
        public ActionResult Index(int? p)
        {
            p = FixPage(p);

            var content = Node as BlogListing;

            var model = new BlogViewModel()
            {
                Content = content
            };

            if (content != null)
            {
                model.Blogs = App.Services.Content.Get<BlogDetails>(Criteria.WithPaging(p.Value, content.BlogPageCount).OrderByDescending(SearchOrder.PublishDate));
            }

            return View(model);
        }

        public ActionResult Details()
        {
            var model = new BlogDetailsViewModel()
            {
                Content = Node as BlogDetails
            };

            return View(model);
        }
    }
}
