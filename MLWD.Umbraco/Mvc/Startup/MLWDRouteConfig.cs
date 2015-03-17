using System.Web.Mvc;
using System.Web.Routing;
using MLWD.Umbraco.Mvc.Routing;

namespace MLWD.Umbraco.Mvc.Startup
{
    public class MLWDRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("media/{*foo}");
            routes.IgnoreRoute("content/{*foo}");

            routes.MapRoute(
                name: "AdminDataTypes",
                url: "admin/syncDataTypes",
                defaults: new { controller = "MLWDAdminCMS", action = "SyncDataTypes" }
            );

            routes.MapRoute(
                name: "AdminSaveDataTypes",
                url: "admin/saveDataTypes",
                defaults: new { controller = "MLWDAdminCMS", action = "SaveDataTypes" }
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

            var umbraco = new Route("{*url}", new RouteValueDictionary() { }, new RouteValueDictionary() { { "url", new MLWDCMSRouteConstraint() } }, new MLWDCMSRouteHandler());

            routes.Add(umbraco);
        }
    }
}