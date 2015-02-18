using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Service.DocTypes
{
    [ContentType(Description = "Services I can offer", Controller = "Services", IconUrl = "icon-settings-alt")]
    public class Services : Page
    {
        
    }
}
