﻿using System.Web;
using Website.Domain.Shared.DocTypes;
using Website.Domain.Shared.Services;
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

        private NodesService _nodes { get; set; }

        public NodesService Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = Services.Get<NodesService>();
                }

                return _nodes;
            }
        }
    }
}
