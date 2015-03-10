using System;
using Yomego.CMS.Core.Mvc.Controllers;

namespace Yomego.CMS.Mvc.Controllers
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
