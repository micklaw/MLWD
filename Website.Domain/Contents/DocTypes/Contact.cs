using System.ComponentModel;
using System.Web;
using Yomego.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;
using Website.Domain.Shared.Ditto.TypeConverters;
using Website.Domain.Shared.DocTypes;

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
    }
}
