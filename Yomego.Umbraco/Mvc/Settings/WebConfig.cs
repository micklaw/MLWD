using System;
using System.Configuration;

namespace Yomego.Umbraco.Mvc.Settings
{
    public class WebConfig
    {
        public WebConfig() { }
   
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