using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Website.Domain.Shared.DocTypes;
using Yomego.Umbraco;

namespace Website.Domain
{
    public class WebsiteApp : App
    {
        public class DomainSettings
        {
            public static string SiteUrl
            {
                get
                {
                    const string format = "{0}://{1}";

                    return string.Format(format, HttpContext.Current.Request.Url.Scheme,
                                         HttpContext.Current.Request.Url.Host);
                }
            }
        }

        private Settings _settings { get; set; }

        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Services.Content.First<Settings>();
                }

                return _settings;
            }
        }
    }
}
