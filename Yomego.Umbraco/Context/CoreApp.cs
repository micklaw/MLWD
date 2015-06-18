using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Context
{
    public class CoreApp<TServiceContainer> : Container where TServiceContainer : CoreServiceContainer, new()
    {
        public TServiceContainer Services { get { return Get<TServiceContainer>(); } }

        public WebContext Context { get { return Get<WebContext>(); } }
    }
}
