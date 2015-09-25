using System.Web;
using System.Web.Mvc;

namespace Website.Domain.Mailer.Models
{
    public class BaseMailerModel
    {
        public ControllerContext ControllerContext { get; set; }

        public HttpContextBase HttpContext { get; set; }

        public string Subject { get; set; }

        public string EmailToAddress { get; set; }
    }
}
