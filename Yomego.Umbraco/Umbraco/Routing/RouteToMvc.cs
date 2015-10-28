using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Umbraco.Model;

namespace Yomego.Umbraco.Umbraco.Routing
{
    /// <summary>
    /// Programatically kick off a web request in to the .Net Mvc pipeline
    /// </summary>
    public class RouteToMvc
    {
        /// <summary>
        /// Kicks of the routing from Umbraco to Mvc
        /// </summary>
        /// <returns></returns>
        public static bool Route(string action = "Index")
        {
            // [ML] - This should be populated via our earlier Content Finders

            var content = HttpContext.Current.Items[Requests.Node] as Content;

            // [ML] - Get the umbraco route attribute if it exists

            var contentType = content?.GetType().GetCustomAttributes(typeof(UmbracoRouteAttribute), true).FirstOrDefault() as UmbracoRouteAttribute;

            /* [ML] - If the page is found in Umbraco then add the node to the request items and route the request to the action and controller set in umbraco */

            if (contentType != null)
            {
                return RouteRequest(contentType, HttpContext.Current, content, action);
            }

            return false;
        }

        private static readonly DefaultControllerFactory _defaultControllerFactory = new DefaultControllerFactory();

        private static readonly RouteCollection _routes = new RouteCollection();

        /// <summary>
        /// Move route values to our new request
        /// </summary>
        /// <param name="routeData"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SetRouteData(RouteData routeData, string key, string value)
        {
            if (routeData.Values[key] == null)
            {
                routeData.Values.Add(key, value);
            }
            else
            {
                routeData.Values[key] = value;
            }
        }

        /// <summary>
        /// Route this request to Mvc
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="httpContext"></param>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool RouteRequest(UmbracoRouteAttribute contentType, HttpContext httpContext, Content content, string action)
        {
            if (contentType != null)
            {
                var httpContextBase = new HttpContextWrapper(httpContext);

                // Get the current route date

                var routeData = _routes.GetRouteData(httpContextBase) ?? new RouteData();

                foreach (var qs in httpContextBase.Request.QueryString.AllKeys)
                {
                    SetRouteData(routeData, qs, httpContextBase.Request.QueryString[qs]);
                }

                SetRouteData(routeData, "controller", contentType.Controller);
                SetRouteData(routeData, "action", contentType.Action ?? "Index"); // [ML] - Default to Index if not populated

                var requestContext = new RequestContext(httpContextBase, routeData);

                var controller = _defaultControllerFactory.CreateController(requestContext, contentType.Controller);

                controller.Execute(requestContext);

                return true;
            }

            return false;
        }
    }
}
