using System;

namespace Yomego.Umbraco.Mvc.Controllers.App
{
    public class UmbracoController<TApp> : BaseController where TApp : class
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
