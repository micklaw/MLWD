using Umbraco.Core;
using Umbraco.Web.Routing;
using Yomego.Umbraco.Umbraco.Routing;

namespace Yomego.Umbraco.Umbraco.Events
{
    /// <summary>
    /// Initialise our custom ContentFinder which overrides the <see cref="ContentFinderByNiceUrl" /> implementation
    /// </summary>
    public class RoutingEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, CustomContentFinder>();

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}
