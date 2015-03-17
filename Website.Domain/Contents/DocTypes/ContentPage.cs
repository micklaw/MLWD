using System.Web;
using MLWD.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Contents.DocTypes
{
    [UmbracoRoute("Content")]
    public class ContentPage : Page
    {
        public ContentPage(IPublishedContent content) : base(content) { }

        public string ContentTitle { get; set; }

        public IHtmlString ContentDescription { get; set; }
    }
}
