using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Routing;
using Yomego.Umbraco.Umbraco.Routing;

namespace Yomego.Umbraco.Umbraco.Events
{
    public class RoutingEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, CustomContentFinder>();
            ContentFinderResolver.Current.RemoveType<ContentFinderByNiceUrl>();

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}
