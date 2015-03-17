using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using MLWD.Umbraco.Constants;
using MLWD.Umbraco.Mvc.Controllers.ActionResults;
using MLWD.Umbraco.Mvc.Model.Interfaces;
using Umbraco.Core.Models.PublishedContent;

namespace MLWD.Umbraco.Mvc.Controllers.Base
{
    public class BaseController : Controller
    {
        #region UmbracoHelpers

        public ActionResult RedirectToNodeParent()
        {
            ActionResult result = new ViewResult();

            if (Node != null && Node.Parent != null)
            {
                if (!string.IsNullOrWhiteSpace(Node.Parent.Url))
                {
                    result = new RedirectResult(Node.Parent.Url);
                }
            }

            return result;
        }

        protected XmlSitemapResult Sitemap(IList<ISitemapItem> sitemap)
        {
            return new XmlSitemapResult(sitemap);
        }

        public PublishedContentModel Node
        {
            get
            {
                return HttpContext.Items[Requests.Node] as PublishedContentModel;
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