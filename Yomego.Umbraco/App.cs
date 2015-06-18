using System;
using System.Web;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Mvc.Settings;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco
{
    public class App : CoreApp<ServiceContainer>
    {
        private readonly Lazy<WebConfig> _webConfig = new Lazy<WebConfig>();

        public WebConfig WebConfig
        {
            get { return _webConfig.Value; }
        }
    }
}
