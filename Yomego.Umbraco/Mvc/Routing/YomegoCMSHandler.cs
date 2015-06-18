using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Mvc.Attributes;

namespace Yomego.Umbraco.Mvc.Routing
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

        public virtual void RouteRequest(UmbracoRouteAttribute contentType, HttpContext httpContext)
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
            var contentType = context.Items[Requests.ContentType] as UmbracoRouteAttribute;;

            if (contentType != null)
            {
                RouteRequest(contentType, context);
            }
        }
    }
}
