using System.Web.Mvc;

namespace Yomego.CMS.Mvc.Controllers
{
    public class SEOCMSController : BaseCMSController
    {

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public ActionResult SiteMap()
        {
            var pages = App.Services.Content.GetSitemapPages(); // ML - Get all pages

            return Sitemap(pages);
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public ActionResult Robots()
        {
            var robots = App.Settings.Robots; // ML - Get all pages

            return Content(robots, "text/plain");
        }
    }
}
