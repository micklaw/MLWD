using System;
using System.Web.Http;

namespace Yomego.Umbraco.Mvc.Controllers.CMS.Api
{
    public class UmbracoApiController<TApp> : ApiController where TApp : class
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
