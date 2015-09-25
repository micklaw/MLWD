using System;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Mvc.Settings;
using Yomego.Umbraco.Umbraco.Services.Container;
using Yomego.Umbraco.Umbraco.Services.Content;

namespace Yomego.Umbraco
{
    public class App : CoreApp<ServiceContainer>
    {
        private readonly Lazy<WebConfig> _webConfig = new Lazy<WebConfig>();

        public WebConfig WebConfig => _webConfig.Value;


        private readonly Lazy<DictionaryService> _dictionary = new Lazy<DictionaryService>();

        public DictionaryService Dictionary => _dictionary.Value;
    }
}
