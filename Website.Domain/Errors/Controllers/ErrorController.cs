using System.Web.Mvc;
using MLWD.Umbraco.Mvc.Controllers.App;
using Website.Domain.Shared.Controllers;

namespace Website.Domain.Errors.Controllers
{
    public class ErrorController : AppController
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
