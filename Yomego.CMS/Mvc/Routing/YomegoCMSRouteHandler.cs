using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Constants;
using Yomego.CMS.Umbraco.Services;

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
