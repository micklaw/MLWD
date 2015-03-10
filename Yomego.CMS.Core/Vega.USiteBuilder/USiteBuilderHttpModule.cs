using System;
using System.Web;

namespace Vega.USiteBuilder
{
    internal class USiteBuilderHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (!Configuration.USiteBuilderConfiguration.SuppressSynchronization)
            {
                context.BeginRequest += BeginRequest;
            }
        }

        void BeginRequest(object sender, EventArgs e)
        {
            UmbracoManager.SynchronizeIfNotSynchronized();
        }

        #endregion
    }
}
