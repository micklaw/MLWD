using System;

namespace Yomego.CMS.Umbraco.Services
{
    public class Service<TApp>
    {
        private Lazy<TApp> LazyApp = new Lazy<TApp>();

        protected TApp App
        {
            get
            {
                return LazyApp.Value;
            }
        }
    }
}
