using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Errors.Controllers
{
    public class ErrorController : BaseCMSController
    {
        public ActionResult Index()
        {
            Response.StatusCode = 500;

            return View();
        }

        public ActionResult Notfound()
        {
            Response.StatusCode = 404;

            return View();
        }
    }
}
