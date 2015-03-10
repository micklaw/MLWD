using System;
using System.Web.Mvc;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Mvc.Controllers
{
    public class OgCMSController : BaseCMSController
    {
        public ActionResult Index(int id)
        {
            // Get the default culture if none is sent

            Content content = null;

            if (id > 0)
            {
                content = App.Services.Content.Get(id);
            }

            if (content == null)
            {
                throw new NullReferenceException(string.Format("Og tags can't be written out as no node can be found for the id {0}", id));
            }

            ViewBag.OgNode = App.Services.Get<OgService>().OgTagFromContent(content);

            return View();
        }
    }
}
