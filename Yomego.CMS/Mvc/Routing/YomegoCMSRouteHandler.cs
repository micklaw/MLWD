using System.Web;
using System.Web.Routing;

namespace Yomego.CMS.Mvc.Routing
{
    public class YomegoCMSRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new YomegoCMSHandler(requestContext);
        }
    }
}
