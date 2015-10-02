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
    /// Alternative approach to our RouteConstraint routing mechanism
    /// </summary>
    public class RouteToMvc
    {
        public static bool Route()
        {
            var content = HttpContext.Current.Items[Requests.Node] as Content;

            // [ML] - Get th umbraco route attribute if it exists

            var contentType = content?.GetType().GetCustomAttributes(typeof(UmbracoRouteAttribute), true).FirstOrDefault() as UmbracoRouteAttribute;

            /* [ML] - If the page is found in Umbraco then add the node to the request items
                 *        and route the request to the action and controller set in umbraco */

            if (contentType != null)
            {
                return RouteRequest(contentType, HttpContext.Current, content);
            }

            return false;
        }

        private static readonly DefaultControllerFactory _defaultControllerFactory = new DefaultControllerFactory();

        private static readonly RouteCollection _routes = new RouteCollection();

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

        public static bool RouteRequest(UmbracoRouteAttribute contentType, HttpContext httpContext, Content content)
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
                SetRouteData(routeData, "action", content.TemplateName ?? "Index"); // [ML] - Default to Index if not populated

                var requestContext = new RequestContext(httpContextBase, routeData);

                var controller = _defaultControllerFactory.CreateController(requestContext, contentType.Controller);

                controller.Execute(requestContext);

                return true;
            }

            return false;
        }
    }
}
