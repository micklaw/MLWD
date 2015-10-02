using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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

            YomegoStartup.Register(config, RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;

            base.OnApplicationStarted(sender, e);
        }
    }
}