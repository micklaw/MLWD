using Mvc.Mailer;
using System;
using System.Net.Mail;
using Website.Domain.Mailer.Models;

namespace Website.Domain.Mailer
{
    public class UserMailer : MailerBase
    {
        private readonly WebsiteApp _app;

        public UserMailer()
        {
            _app = new WebsiteApp();
        }

        public MvcMailMessage ContactUs(ContactMailerModel model)
        {
            CurrentHttpContext = model.HttpContext;
            ControllerContext = model.ControllerContext;

            var mailMessage = new MvcMailMessage
            {
                Subject = model.Subject,
                From = new MailAddress(_app.Settings.CompanyEmail, _app.Settings.CompanyName)
            };

            mailMessage.To.Add(model.EmailToAddress ?? _app.Settings.CompanyEmail);

            ViewBag.Form = model.Form;

            PopulateBody(mailMessage, "ContactUs");

            return mailMessage;
        }
    }
}
