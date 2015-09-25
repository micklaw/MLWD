using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yomego.Umbraco.Constants;

namespace Website.Domain.Contents.Form
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }

        public string Telephone { get; set; }

        [RegularExpression(Regex.EmailRegex, ErrorMessage = "Please enter a valid email")]
        [Required(ErrorMessage = "Please enter an email.")]
        public string Email { get; set; }

        public string Company { get; set; }

        [Required(ErrorMessage = "Please enter a good message.")]
        public string Message { get; set; }
    }
}
