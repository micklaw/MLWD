using System.Web.Http;
using System.Web.Routing;
using Yomego.Umbraco.Mvc.Serializing;
using Yomego.Umbraco.Mvc.Serializing.Converters;
using Yomego.Umbraco.Mvc.Startup;
using Yomego.Umbraco.Umbraco.Services.Content;
using Yomego.Umbraco.Umbraco.Services.DataTypes;

namespace Yomego.Umbraco
{
    public class YomegoStartup
    {
        public static void Register(RouteCollection routes, HttpConfiguration config)
        {
            // Hook up Umbraco plugin
            App.ResolveUsing<ContentService, UmbracoContentService>();
            App.ResolveUsing<DataTypeService, UmbracoDataTypeService>();

            YomegoRouteConfig.RegisterRoutes(routes);
            YomegoApiConfig.Register(config);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new ExcludeContractResolver(new[]
            {
                "Children",
                "ContentSet",
                "ContentType",
                "ItemType",
                "Parent",
                "Properties",
                "properties",
                "this",
                "Content"
            });

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new HtmlStringConverter());
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}