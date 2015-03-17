using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace MLWD.Umbraco.Mvc.Controllers.App
{
    public class BaseCMSController : UmbracoController<MLWD.Umbraco.App>
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FixUmbracoContext();
            SetRequestCulture(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private void FixUmbracoContext()
        {
            if (Node == null)
            {
                var appcontext = global::Umbraco.Core.ApplicationContext.Current;

                UmbracoContext.EnsureContext(HttpContext, appcontext, new WebSecurity(HttpContext, appcontext));
            }
        }
    }
}
