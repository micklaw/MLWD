using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Core.Sync;
using Umbraco.Web;

namespace Yomego.Umbraco.Umbraco.Events
{
    public class LoadBalancingEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ServerMessengerResolver.Current.SetServerMessenger(new BatchedDatabaseServerMessenger(
                applicationContext,
                true,
                new DatabaseServerMessengerOptions
                {
                    InitializingCallbacks = new Action[]
                    {
                        () => global::umbraco.content.Instance.RefreshContentFromDatabase(),
                        () => Examine.ExamineManager.Instance.RebuildIndex()
                    }
                }));

            ServerRegistrarResolver.Current.SetServerRegistrar(new DatabaseServerRegistrar(new Lazy<IServerRegistrationService>(() => applicationContext.Services.ServerRegistrationService), new DatabaseServerRegistrarOptions()));
        }
    }
}
