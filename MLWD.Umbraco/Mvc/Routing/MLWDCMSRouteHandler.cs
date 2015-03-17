using System.Web;
using System.Web.Routing;

namespace MLWD.Umbraco.Mvc.Routing
{
    public class MLWDCMSRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MLWDCMSHandler(requestContext);
        }
    }
}
