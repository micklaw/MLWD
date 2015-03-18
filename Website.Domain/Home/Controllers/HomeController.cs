using System.Web.Mvc;
using MLWD.Umbraco.Collections;
using MLWD.Umbraco.Mvc.Controllers.App;
using MLWD.Umbraco.Umbraco.Services.Search.Enums;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Home.DocTypes;
using Website.Domain.Home.ViewModels;
using Website.Domain.Shared.Controllers;
using Website.Domain.Shared.Search;

namespace Website.Domain.Home.Controllers
{
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            var searchCriteriaBlog = SearchCriteria.WithExcludeBlogCategory("work").AndPaging(0, 4).OrderByDescending(SearchOrder.PublishDate);
            var searchCriteriaWork = SearchCriteria.WithBlogCategory("work").AndPaging(0, 6).OrderByDescending(SearchOrder.PublishDate);

            var model = new HomeViewModel
            {
                Content = Node as Homepage,
                Blogs = App.Services.Content.Get<BlogDetails>(searchCriteriaBlog) ?? new PagedList<BlogDetails>(),
                Work = App.Services.Content.Get<BlogDetails>(searchCriteriaWork) ?? new PagedList<BlogDetails>(),
            };

            return View(model);
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public ActionResult SiteMap()
        {
            var pages = App.Services.Content.GetSitemapPages();

            return Sitemap(pages);
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public ActionResult Robots()
        {
            var robots = App.Settings.Robots; 

            return Content(robots, "text/plain");
        }
    }
}
