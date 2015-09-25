using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Routing;
using Archetype.Models;
using Newtonsoft.Json;
using Umbraco.Core.Models.PublishedContent;
using Yomego.Umbraco.Mvc.Serializing;
using Yomego.Umbraco.Mvc.Serializing.Converters;
using Yomego.Umbraco.Mvc.Startup;
using Yomego.Umbraco.Umbraco.Services.Content;
using Yomego.Umbraco.Umbraco.Services.DataTypes;

namespace Yomego.Umbraco
{
    public class YomegoStartup
    {
        public static void Register(HttpConfiguration config, RouteCollection routes, bool withCatchAll = true)
        {
            // Hook up Umbraco plugin
            App.ResolveUsing<ContentService, UmbracoContentService>();
            App.ResolveUsing<DataTypeService, UmbracoDataTypeService>();

            YomegoRouteConfig.RegisterRoutes(routes, withCatchAll);
            YomegoApiConfig.Register(config);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new ExcludeContractResolver(new Dictionary<Type, IList<string>>
            {
                {
                    typeof (PublishedContentModel), new List<string>()
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
                    }
                },
                {
                    typeof (ArchetypeFieldsetModel), new List<string>()
                    {
                        "Properties",
                        "properties"
                    }
                }
            });

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new HtmlStringConverter());
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}