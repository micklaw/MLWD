using Yomego.CMS.Core.Context;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Context
{
    public class CoreApp<TServiceContainer> : Container where TServiceContainer : CoreServiceContainer, new()
    {
        public TServiceContainer Services { get { return Get<TServiceContainer>(); } }

        public WebContext Context { get { return Get<WebContext>(); } }
    }
}
