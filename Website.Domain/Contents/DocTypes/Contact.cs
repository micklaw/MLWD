using System.Web;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;

namespace Website.Domain.Contents.DocTypes
{
    [UmbracoRoute("Content", "Contact")]
    public class Contact : Page
    {
        public Contact(IPublishedContent content) : base(content) { }

        public string ContactMap { get; set; }

        public string ContactTitle { get; set; }

        public IHtmlString ContactDescription { get; set; }
    }
}
