using System.Web.Mvc;
using Website.Domain.Service.ViewModels;
using Website.Domain.Shared.Controllers;

namespace Website.Domain.Service.Controllers
{
    public class ServicesController : AppController
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
