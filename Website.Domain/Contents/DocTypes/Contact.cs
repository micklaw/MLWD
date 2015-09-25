using System.ComponentModel;
using System.Web;
using Yomego.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Website.Domain.Shared.DocTypes;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Contents.DocTypes
{
    [UmbracoRoute("Content", "Contact")]
    public class Contact : Page
    {
        public Contact(IPublishedContent content) : base(content) { }

        public string ContactMap { get; set; }

        public string ContactTitle { get; set; }

        [TypeConverter(typeof(HtmlStringConverter))]
        public HtmlString ContactDescription { get; set; }

        public string ThanksTitle { get; set; }

        [TypeConverter(typeof(HtmlStringConverter))]
        public HtmlString ThanksDescription { get; set; }
    }
}
