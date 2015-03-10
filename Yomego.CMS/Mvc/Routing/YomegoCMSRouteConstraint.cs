using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Constants;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Mvc.Routing
{
    public class YomegoCMSRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return IsRoutable(httpContext);
        }

        public bool IsRoutable(HttpContextBase context)
        {
            // [ML] - Try and get the content from the umbraco Api

            var app = new CoreApp<CoreServiceContainer>();

            var content = app.Services.Content.Get(HttpContext.Current.Request.RawUrl.Split('?')[0]);

            if (content != null)
            {
                var contentType = content.GetType().GetCustomAttributes(typeof(ContentTypeAttribute), true).FirstOrDefault() as ContentTypeAttribute;

                /* [ML] - If the page is found in Umbraco then add the node to the request items
                 *        and route the request to the action and controller set in umbraco */

                if (contentType != null)
                {
                    if (string.IsNullOrWhiteSpace(contentType.Controller))
                    {
                        throw new ArgumentNullException(string.Format(
                            "The controller must be defined for type ({0}) to route.", content.GetType().Name));
                    }

                    context.Items[Requests.Node] = content;
                    context.Items[Requests.PageId] = content.Id; // [ML] - Required to hook up umbraco Jazz
                    context.Items[Requests.ContentType] = contentType;

                    return true;
                }
            }

            return false;
        }
    }
}