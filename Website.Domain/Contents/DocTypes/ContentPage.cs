using System.ComponentModel;
using System.Web;
using Yomego.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Website.Domain.Shared.Ditto.TypeConverters;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Contents.DocTypes
{
    [UmbracoRoute("Content")]
    public class ContentPage : Page
    {
        public ContentPage(IPublishedContent content) : base(content) { }

        public string ContentTitle { get; set; }

        [TypeConverter(typeof(HtmlStringConverter))]
        public HtmlString ContentDescription { get; set; }
    }
}
