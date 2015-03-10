using System.Web.Mvc;
using Website.Domain.Contents.DocTypes;
using Website.Domain.Contents.ViewModels;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Contents.Controllers
{
    public class ContentController : BaseCMSController
    {
        public ActionResult Content()
        {
            var model = new ContentViewModel()
            {
                Content = Node as ContentPage
            };

            return View(model);
        }

        public ActionResult Contact()
        {
            var model = new ContactViewModel()
            {
                Content = Node as Contact
            };

            return View(model);
        }
    }
}