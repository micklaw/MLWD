using System;
using System.Web.Http;
using System.Web.Routing;
using Vega.USiteBuilder;
using Yomego.CMS.Mvc.Startup;
using Yomego.CMS.Umbraco.Startup;
using UmbracoWeb = Umbraco.Web;

namespace Yomego.CMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : UmbracoWeb.UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;

            YomegoCMSBuildersConfig.Register();
            YomegoCMSDependencyConfig.Register(); 

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            base.OnApplicationStarted(sender, e);
        }
    }
}