using System;

namespace Yomego.Umbraco.Mvc.Controllers.App
{
    public class UmbracoController<TApp> : BaseController where TApp : Yomego.Umbraco.App
    {
        private readonly Lazy<TApp> _lazyApp = new Lazy<TApp>();

        protected TApp App => _lazyApp.Value;
    }
}
