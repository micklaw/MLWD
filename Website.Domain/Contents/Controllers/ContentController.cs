using System.Web.Mvc;
using MLWD.Umbraco.Mvc.Controllers.App;
using Website.Domain.Contents.DocTypes;
using Website.Domain.Contents.ViewModels;
using Website.Domain.Shared.Controllers;

namespace Website.Domain.Contents.Controllers
{
    public class ContentController : AppController
    {
        public ActionResult Index()
        {
            var model = new ContentViewModel()
            {
                Content = Node as ContentPage
            };

            return View(model);
        }
    }
}