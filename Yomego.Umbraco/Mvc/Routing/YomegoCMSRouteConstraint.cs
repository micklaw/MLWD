using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Mvc.Routing
{
    public class YomegoCMSRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsRoutable(httpContext);
        }

        private bool PopulateContext(CoreApp<CoreServiceContainer> app, HttpContextBase context, string url, UmbracoRouteAttribute contentType = null)
        {
            // [ML ] -If this request has previously been routed, do nothing and route as context is populated

            if (context.Items[Requests.Routed] == null)
            {
                // [ML] - if this is a new request try and find the content

                PublishedContentModel content = null;

                if (UmbracoContext.Current != null)
                {
                    try
                    {
                        var item = app.Services.Content.Get(url);

                        if (item != null)
                        {
                            content = item as PublishedContentModel;
                        }
                    }
                    catch
                    {
                        // [ML] - Suppress as if there is no context then this will blow up
                    }
                }

                if (content != null)
                {
                    // [ML] - Dont get the content type if we already have it

                    if (contentType == null)
                    {
                        contentType =
                            content.GetType().GetCustomAttributes(typeof(UmbracoRouteAttribute), true).FirstOrDefault()
                                as UmbracoRouteAttribute;
                    }

                    /* [ML] - If the page is found in Umbraco then add the node to the request items
                     *        and route the request to the action and controller set in umbraco */

                    if (contentType != null)
                    {
                        YomegoRouteTable.AddRoute(url, contentType);

                        if (string.IsNullOrWhiteSpace(contentType.Controller))
                        {
                            throw new ArgumentNullException(string.Format(
                                "The controller must be defined for type ({0}) to route.", content.GetType().Name));
                        }

                        context.Items[Requests.Node] = content;
                        context.Items[Requests.PageId] = content.Id; // [ML] - Required to hook up umbraco Jazz
                        context.Items[Requests.ContentType] = contentType;
                        context.Items[Requests.Routed] = true;

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRoutable(HttpContextBase context)
        {
            // [ML] - Try and get the content from the umbraco Api
            var app = new CoreApp<CoreServiceContainer>();

            var url = HttpContext.Current.Request.RawUrl.Split('?')[0];

            var contentType = YomegoRouteTable.GetFromUrl(url);

            return PopulateContext(app, context, url, contentType);
        }
    }
}