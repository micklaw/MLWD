using System.Collections.Concurrent;
using Yomego.Umbraco.Mvc.Attributes;

namespace Yomego.Umbraco.Mvc.Routing
{
    public static class YomegoRouteTable
    {
        public static ConcurrentDictionary<string, UmbracoRouteAttribute> _routes = new ConcurrentDictionary<string, UmbracoRouteAttribute>();

        public static UmbracoRouteAttribute GetFromUrl(string url)
        {
            UmbracoRouteAttribute attribute;

            _routes.TryGetValue(url.Split(',')[0].ToLower(), out attribute);

            return attribute;
        }

        public static bool AddRoute(string url, UmbracoRouteAttribute attribute)
        {
            return _routes.TryAdd(url.Split(',')[0].ToLower(), attribute);
        }
    }
}
