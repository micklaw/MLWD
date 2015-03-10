using System.Configuration;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Umbraco.DocumentTypes;
using Yomego.CMS.Core.Umbraco.Search;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS
{
    public class App : CoreApp<ServiceContainer>
    {
        private Settings _settings { get; set; }

        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Services.Content.First<Settings>(Criteria.WithCulture(null));
                }

                return _settings;
            }
        }

        #region Web Config Settings

        public class WebConfig
        {
            private WebConfig() { }

            private WebConfig _current { get; set; }

            public WebConfig Current
            {
                get
                {
                    if (_current == null)
                    {
                        _current = new WebConfig();
                    }

                    return _current;
                }
            }
                
            public string GetString(string key)
            {
                return ConfigurationManager.AppSettings[key];
            }

            public int GetInt(string key)
            {
                int value;
                int.TryParse(GetString(key), out value);

                return value;
            }

            public bool GetBool(string key)
            {
                bool value;
                bool.TryParse(GetString(key), out value);

                return value;
            }
        }

        #endregion Web Config Settings
    }
}
