using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using Umbraco.Core.Models;

namespace Website.Domain.Service.DocTypes
{
    [UmbracoRoute("Services")]
    public class Services : Page
    {
        public Services(IPublishedContent content) : base(content)
        {
        }
    }
}
