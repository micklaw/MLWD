using System;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Mvc.Settings;
using Yomego.Umbraco.Umbraco.Services.Container;
using Yomego.Umbraco.Umbraco.Services.Content;
using Yomego.Umbraco.Umbraco.Services.DataTypes;

namespace Yomego.Umbraco
{
    public class App : CoreApp<ServiceContainer>
    {
        public App()
        {
            // Hook up Umbraco plugin
            App.ResolveUsing<ContentService, UmbracoContentService>();
            App.ResolveUsing<DataTypeService, UmbracoDataTypeService>();
        }

        private readonly Lazy<WebConfig> _webConfig = new Lazy<WebConfig>();

        public WebConfig WebConfig => _webConfig.Value;


        private readonly Lazy<DictionaryService> _dictionary = new Lazy<DictionaryService>();

        public DictionaryService Dictionary => _dictionary.Value;
    }
}
