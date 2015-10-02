using System.Web.Mvc;
using System.Web.Routing;

namespace Yomego.Umbraco.Mvc.Startup
{
    public class YomegoRouteConfig
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
                defaults: new { controller = "YomegoAdminCMS", action = "SyncDataTypes" }
            );

            routes.MapRoute(
                name: "AdminSaveDataTypes",
                url: "admin/saveDataTypes",
                defaults: new { controller = "YomegoAdminCMS", action = "SaveDataTypes" }
            );
        }
    }
}