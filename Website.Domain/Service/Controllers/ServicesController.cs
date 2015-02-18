using System.Web.Mvc;
using Website.Domain.Service.ViewModels;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Service.Controllers
{
    public class ServicesController : BaseCMSController
    {
        public ActionResult Index()
        {
            var model = new ServicesViewModel()
            {
                Content = Node as DocTypes.Services
            };

            return View(model);
        }
    }
}
