using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Yomego.CMS.Core.Constants;
using Yomego.CMS.Core.Mvc.ActionResults;
using Yomego.CMS.Core.Mvc.Models.Interfaces;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Core.Umbraco.Model.Interfaces;

namespace Yomego.CMS.Core.Mvc.Controllers
{
    public class BaseController : Controller
    {
        #region UmbracoHelpers

        public ActionResult RedirectToNodeParent()
        {
            ActionResult result = new ViewResult();

            if (Node != null)
            {
                if (!string.IsNullOrWhiteSpace(Node.ParentUrl))
                {
                    result = new RedirectResult(Node.ParentUrl);
                }
            }

            return result;
        }

        protected ActionResult CMS(Content content, object data)
        {
            var viewableContent = content as IViewable;

            if (viewableContent != null && !string.IsNullOrWhiteSpace(viewableContent.MvcViewName))
            {
                return View(viewableContent.MvcViewName, data);
            }

            return View(data);
        }

        protected XmlSitemapResult Sitemap(IList<ISitemapItem> sitemap)
        {
            return new XmlSitemapResult(sitemap);
        }

        public Content Node
        {
            get
            {
                return HttpContext.Items[Requests.Node] as Content;
            }
        }

        private void CheckForSEOContent()
        {
            var page = Node as Page;

            if (page != null)
            {
                ViewBag.PageTitle = page.MetaPageTitle;

                if (string.IsNullOrWhiteSpace(page.MetaPageTitle))
                {
                    ViewBag.PageTitle = page.Name;
                }

                ViewBag.MetaDescription = page.MetaDescription;
                ViewBag.MetaKeywords = page.MetaKeywords;
            }
        }

        #endregion UmbracoHelpers

        #region partial rendering in controller

        public string RenderPartialToString(string partialViewName, object model)
        {
            InvalidateControllerContext();
            var view = ViewEngines.Engines.FindPartialView(ControllerContext, partialViewName).View;

            string result = RenderViewToString(view, model);
            return result;
        }

        public string RenderViewToString(string viewName, object model)
        {
            InvalidateControllerContext();
            var view = ViewEngines.Engines.FindView(ControllerContext, viewName, null).View;

            string result = RenderViewToString(view, model);
            return result;
        }

        public string RenderViewToString(IView view, object model)
        {
            InvalidateControllerContext();
            string result = null;

            if (view != null)
            {
                var sb = new StringBuilder();

                using (var writer = new StringWriter(sb))
                {
                    var viewContext = new ViewContext(ControllerContext, view, new ViewDataDictionary(model), new TempDataDictionary(), writer);

                    view.Render(viewContext, writer);
                    writer.Flush();
                }

                result = sb.ToString();
            }
            return result;
        }

        private void InvalidateControllerContext()
        {
            if (ControllerContext == null)
            {
                var context = new ControllerContext(System.Web.HttpContext.Current.Request.RequestContext, this);
                ControllerContext = context;
            }
        }

        #endregion

        #region Json Overrides

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonNetResult(data, contentType, contentEncoding);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult(data, contentType, contentEncoding, behavior);
        }

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckForSEOContent();

            base.OnActionExecuting(filterContext);
        }

        public int FixPage(int? page)
        {
            if (!page.HasValue)
            {
                return 0;
            }

            return page.Value - 1;
        }
    }
}