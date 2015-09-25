using System.Web;
using Umbraco.Web.Security;
using static Umbraco.Web.UmbracoContext;
using HttpContext = System.Web.HttpContext;
#pragma warning disable 618

namespace Yomego.Umbraco.Umbraco.Modules
{
    public class YomegoModule : IHttpHandler
    {
        private static void FixUmbracoContext()
        {
            var appcontext = global::Umbraco.Core.ApplicationContext.Current;

            if (appcontext != null && HttpContext.Current != null)
            {
                var context = new HttpContextWrapper(HttpContext.Current);

                EnsureContext(context, appcontext, new WebSecurity(context, appcontext));
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            FixUmbracoContext();
        }

        public bool IsReusable { get; } = false;
    }
}
