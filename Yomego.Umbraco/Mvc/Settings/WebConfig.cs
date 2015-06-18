using System;
using System.Configuration;

namespace Yomego.Umbraco.Mvc.Settings
{
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
            Int32.TryParse(GetString(key), out value);

            return value;
        }

        public bool GetBool(string key)
        {
            bool value;
            Boolean.TryParse(GetString(key), out value);

            return value;
        }
    }
}