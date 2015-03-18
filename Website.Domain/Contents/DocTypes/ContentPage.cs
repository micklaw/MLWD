using System.Web;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using Umbraco.Core.Models;

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
