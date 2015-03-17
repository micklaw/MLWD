using MLWD.Umbraco.Mvc.Attributes;
using Umbraco.Core.Models;
using Website.Domain.Shared.DocTypes;

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
