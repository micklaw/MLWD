using System.Globalization;
using System.Net.Mime;
using System.Threading;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Security;
using Yomego.CMS.Constants;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Umbraco.Services;
using UmbracoCore = Umbraco.Core;

namespace Yomego.CMS.Mvc.Controllers
{
    public class BaseCMSController : UmbracoController<App>
    {
        private void SetRequestCulture(ActionExecutingContext filterContext)
        {
            if (Node != null && Node.Id > 0)
            {
                ViewBag.CurrentCulture = App.Context.CurrentCulture;
            }
            else
            {
                object culture = filterContext.RouteData.Values["culture"];

                if (culture != null)
                {
                    var cultureString = culture.ToString();

                    ViewBag.CurrentCulture = cultureString;

                    var currentCulture = new CultureInfo("en-GB");

                    try
                    {
                        currentCulture = new CultureInfo(cultureString.Trim(' '));
                    }
                    catch
                    {
                        // TODO: ELMAH - [ML] - Suppressed as some random cultures throw exceptions if not installed on box, if so set default culture
                    }

                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }
            }
        }

        private void WriteOgTags()
        {
            ViewBag.OgNode = App.Services.Get<OgService>().OgTagFromContent(Node);
        }

        private void WriteCanonical()
        {
            if (Node != null)
            {
                var node = Node as Page;

                if (node != null)
                {
                    ViewBag.Canonical = App.Context.DomainUrl + node.Url;
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FixUmbracoContext();

            WriteOgTags();
            WriteCanonical();

            SetRequestCulture(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private void FixUmbracoContext()
        {
            if (Node == null)
            {
                var appcontext = UmbracoCore.ApplicationContext.Current;

                UmbracoContext.EnsureContext(HttpContext, appcontext, new WebSecurity(HttpContext, appcontext));
            }
        }
    }
}
