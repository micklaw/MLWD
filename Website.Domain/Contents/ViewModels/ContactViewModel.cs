using Website.Domain.Contents.DocTypes;
using Website.Domain.Contents.Form;

namespace Website.Domain.Contents.ViewModels
{
    public class ContactViewModel
    {
        public Contact Content { get; set; }

        public bool Success { get; set; }

        public ContactForm Form { get; set; } = new ContactForm();
    }
}