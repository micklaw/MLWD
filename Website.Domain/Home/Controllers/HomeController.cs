using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Website.Domain.Home.DocTypes;
using Website.Domain.Home.ViewModels;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Home.Controllers
{
    public class HomeController : BaseCMSController
    {
        public ActionResult Index()
        {
            var model = new HomeViewModel()
            {
                Content = Node as Homepage
            };

            return View(model);
        }
    }
}
