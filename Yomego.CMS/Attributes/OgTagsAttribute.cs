using System;
using System.Web;
using System.Web.Mvc;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Attributes
{
    public class OgTagsAttribute : ActionFilterAttribute
    {
        private Lazy<App>_lazy = new Lazy<App>(); 

        private App App
        {
            get { return _lazy.Value; }
        }

        public Content Node
        {
            get
            {
                return HttpContext.Current.Items["Node"] as Content;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Node != null && Node.Id > 0)
            {
                filterContext.Controller.ViewBag.OgNode = App.Services.Get<OgService>().OgTagFromContent(Node);
            }
        }
    }
}
