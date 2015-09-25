using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Routing;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Umbraco.Routing
{
    /// <summary>
    /// Alternative approach to our RouteConstraint routing mechanism
    /// </summary>
    public class RouteToMvc
    {
        public static void Route()
        {
            // [ML - If this is an Umbraco request

            var publishedContent = UmbracoContext.Current.PublishedContentRequest?.PublishedContent;

            if (publishedContent != null)
            {
                var context = HttpContext.Current;
                var app = new CoreApp<CoreServiceContainer>();

                var content = app.Services.Content.Get(UmbracoContext.Current.PublishedContentRequest.PublishedContent) as PublishedContentModel;

                // [ML] - Dont get the content type if we already have it

                var contentType = content?.GetType().GetCustomAttributes(typeof (UmbracoRouteAttribute), true).FirstOrDefault() as UmbracoRouteAttribute;

                /* [ML] - If the page is found in Umbraco then add the node to the request items
                 *        and route the request to the action and controller set in umbraco */

                context.Items[Requests.Node] = content;

                if (contentType != null)
                {
                    context.Items[Requests.ContentType] = contentType;

                    RouteRequest(contentType, context);
                }
            }
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

        public static void RouteRequest(UmbracoRouteAttribute contentType, HttpContext httpContext)
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
            }
        }
    }
}
