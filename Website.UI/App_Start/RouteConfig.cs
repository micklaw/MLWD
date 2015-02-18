using System.Web.Mvc;
using System.Web.Routing;
using Yomego.CMS.Mvc.Startup;

namespace Website.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            YomegoCMSRouteConfig.RegisterRoutes(routes);
        }
    }
}