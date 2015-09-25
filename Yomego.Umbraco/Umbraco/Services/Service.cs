using System;

namespace Yomego.Umbraco.Umbraco.Services
{
    public class Service<TApp>
    {
        private readonly Lazy<TApp> _lazyApp = new Lazy<TApp>();

        protected TApp App => _lazyApp.Value;
    }
}
