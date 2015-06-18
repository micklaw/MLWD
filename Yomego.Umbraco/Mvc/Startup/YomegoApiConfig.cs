using System.Web.Http;

namespace Yomego.Umbraco.Mvc.Startup
{
    public static class YomegoApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CMSApiSearch",
                routeTemplate: "api/content/search",
                defaults: new { controller = "DocumentApi", action = "Search" }
            );

            config.Routes.MapHttpRoute(
                name: "CMSApiFirst",
                routeTemplate: "api/content/first",
                defaults: new { controller = "DocumentApi", action = "First" }
            );

            config.Routes.MapHttpRoute(
                name: "CMSDefaultApi",
                routeTemplate: "api/content/{id}",
                defaults: new { controller = "DocumentApi", id = RouteParameter.Optional }
            );
        }
    }
}
