
using Website.Domain.Contents.Form;

namespace Website.Domain.Mailer.Models
{
    public class ContactMailerModel : BaseMailerModel
    {
        public ContactForm Form { get; set; }
    }
}
