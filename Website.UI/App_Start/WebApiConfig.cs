using System.Web.Http;
using Yomego.CMS.Mvc.Startup;

namespace Website.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            YomegoCMSApiConfig.Register(config);
        }
    }
}
