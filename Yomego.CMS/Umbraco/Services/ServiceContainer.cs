using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Umbraco.Services
{
    public class ServiceContainer : CoreServiceContainer
    {
        public ServiceContainer()
        {
            Content.AfterModelBound += Content_AfterModelBound;
        }

        void Content_AfterModelBound(Content content)
        {

        }
    }
}
