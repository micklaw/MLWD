using System.Web.Mvc;
using Website.Domain.Contents.DocTypes;
using Website.Domain.Contents.Form;
using Website.Domain.Contents.ViewModels;
using Website.Domain.Mailer;
using Website.Domain.Mailer.Models;
using Website.Domain.Shared.Controllers;

namespace Website.Domain.Contents.Controllers
{
    public class ContentController : AppController
    {
        private UserMailer _emailer;

        public ContentController()
        {
            _emailer = new UserMailer();
        }

        public ActionResult Index()
        {
            var model = new ContentViewModel()
            {
                Content = Node as ContentPage
            };

            return View(model);
        }

        public ActionResult Contact(bool success = false)
        {
            var model = new ContactViewModel()
            {
                Content = Node as Contact,
                Success = success
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Contact(ContactForm form)
        {
            if (ModelState.IsValid)
            {
                _emailer.ContactUs(new ContactMailerModel()
                {
                    Form = form,
                    ControllerContext = ControllerContext,
                    HttpContext = HttpContext,
                    Subject = "Someone wants you for something"
                })
                .Send();

                return Contact(true);
            }

            return Contact();
        }
    }
}