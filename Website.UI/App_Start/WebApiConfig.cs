using System.Web.Http;
using MLWD.Umbraco.Mvc.Startup;

namespace Website.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            MLWDApiConfig.Register(config);
        }
    }
}
