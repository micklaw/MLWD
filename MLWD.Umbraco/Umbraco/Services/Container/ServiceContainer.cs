using Umbraco.Core.Models.PublishedContent;

namespace MLWD.Umbraco.Umbraco.Services.Container
{
    public class ServiceContainer : CoreServiceContainer
    {
        public ServiceContainer()
        {
            Content.AfterModelBound += Content_AfterModelBound;
        }

        void Content_AfterModelBound(PublishedContentModel content)
        {

        }
    }
}
