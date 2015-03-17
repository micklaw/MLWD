using System.Web.Mvc;
using System.Web.Routing;
using MLWD.Umbraco.Mvc.Startup;

namespace Website.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            MLWDRouteConfig.RegisterRoutes(routes);

            routes.MapRoute(
                name: "Error",
                url: "error",
                defaults: new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
                name: "404",
                url: "error/not-found",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }
    }
}