using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Home.DocTypes;
using Website.Domain.Home.ViewModels;
using Website.Domain.Shared.Controllers;
using Website.Domain.Shared.Search;
using Website.Domain.Sitemap.Services;
using Yomego.Umbraco;
using Yomego.Umbraco.Collections;
using Yomego.Umbraco.Umbraco.Services.Search.Enums;

namespace Website.Domain.Home.Controllers
{
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            var searchCriteriaBlog =
                SearchCriteria.WithExcludeBlogCategory("work")
                    .AndPaging(0, 4)
                    .OrderByDescending(SearchOrder.PublishDate);
            var searchCriteriaWork =
                SearchCriteria.WithBlogCategory("work").AndPaging(0, 6).OrderByDescending(SearchOrder.PublishDate);

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
            var pages = App.Services.Get<SitemapService>().GetSitemapPages();

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
