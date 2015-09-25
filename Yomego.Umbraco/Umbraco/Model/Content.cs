using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Yomego.Umbraco.Umbraco.Ditto.Resolvers.TemplateName;

namespace Yomego.Umbraco.Umbraco.Model
{
    public class Content : PublishedContentModel
    {
        public Content(IPublishedContent content) : base(content)
        {
            
        }

        [DittoValueResolver(typeof(TemplateNameValueResolver))]
        public string TemplateName { get; set; }
    }
}
