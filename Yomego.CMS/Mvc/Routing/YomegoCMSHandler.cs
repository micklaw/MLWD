using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Constants;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Mvc.Routing
{
    public class YomegoCMSHandler : IHttpHandler
    {
        public bool IsReusable { get; private set; }

        private RequestContext _requestContext { get; set; }

        public YomegoCMSHandler(RequestContext requestContext)
        {
            _requestContext = requestContext;

            IsReusable = true;
        }

        public virtual bool IsRoutable(HttpContext context, out ContentTypeAttribute contentType)
        {
            contentType = context.Items[Requests.ContentType] as ContentTypeAttribute;

            return contentType != null;
        }

        #region MVC route helpers

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

        #endregion MVC route helpers

        public virtual void RouteRequest(ContentTypeAttribute contentType, HttpContext httpContext)
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
                SetRouteData(routeData, "action", contentType.Action ?? "Index");

                var requestContext = new RequestContext(httpContextBase, routeData);

                var controller = _defaultControllerFactory.CreateController(requestContext, contentType.Controller);

                controller.Execute(requestContext);
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            ContentTypeAttribute contentType;

            if (IsRoutable(context, out contentType))
            {
                RouteRequest(contentType, context);
            }
        }
    }
}
