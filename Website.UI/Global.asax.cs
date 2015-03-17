using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MLWD.Umbraco.Umbraco.Startup;

namespace Website.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : Umbraco.Web.UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MLWDDependencyConfig.Register();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            base.OnApplicationStarted(sender, e);
        }
    }
}