using System.Web.Http;

namespace Yomego.CMS.Mvc.Startup
{
    public static class YomegoCMSApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "YomegoCMSApiSearch",
                routeTemplate: "api/content/search",
                defaults: new { controller = "DocumentApi", action = "Search" }
            );

            config.Routes.MapHttpRoute(
                name: "YomegoCMSApiFirst",
                routeTemplate: "api/content/first",
                defaults: new { controller = "DocumentApi", action = "First" }
            );

            config.Routes.MapHttpRoute(
                name: "YomegoCMSDefaultApi",
                routeTemplate: "api/content/{id}",
                defaults: new { controller = "DocumentApi", id = RouteParameter.Optional }
            );
        }
    }
}
