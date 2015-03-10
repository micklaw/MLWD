using System.Web.Mvc;
using System.Web.Routing;
using Yomego.CMS.Mvc.Routing;

namespace Yomego.CMS.Mvc.Startup
{
    public class YomegoCMSRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("media/{*foo}");
            routes.IgnoreRoute("content/{*foo}");

            routes.MapRoute(
                name: "AdminDocTypes",
                url: "admin/syncnow",
                defaults: new { controller = "YomegoAdminCMS", action = "Sync" }
            );

            routes.MapRoute(
                name: "AdminDataTypes",
                url: "admin/syncDataTypes",
                defaults: new { controller = "YomegoAdminCMS", action = "SyncDataTypes" }
            );

            routes.MapRoute(
                name: "AdminSaveDataTypes",
                url: "admin/saveDataTypes",
                defaults: new { controller = "YomegoAdminCMS", action = "SaveDataTypes" }
            );

            routes.MapRoute(
                name: "Sitemap",
                url: "sitemap.xml",
                defaults: new { controller = "SEOCMS", action = "Sitemap" }
            );

            routes.MapRoute(
                name: "Robots",
                url: "robots.txt",
                defaults: new { controller = "SEOCMS", action = "Robots" }
            );

            routes.MapRoute(
                name: "Og",
                url: "og",
                defaults: new { controller = "OgCMS", action = "Index" }
            );

            var catchAll = new Route("{*url}", new RouteValueDictionary(), new YomegoCMSRouteHandler());

            routes.Add(catchAll);
        }
    }
}