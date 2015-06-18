using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Website.Domain.Shared.Serializing;
using Website.Domain.Shared.Serializing.Converters;
using Yomego.Umbraco;

namespace Website.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : Umbraco.Web.UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            var config = GlobalConfiguration.Configuration;

            WebApiConfig.Register(config);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            YomegoStartup.Register(RouteTable.Routes, config);

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

            base.OnApplicationStarted(sender, e);
        }
    }
}