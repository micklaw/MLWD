using System.Reflection;
using umbraco.BusinessLogic;

namespace Vega.USiteBuilder
{
    /// <summary>
    /// Internal class
    /// </summary>
    public class SiteBuilderApplicationBase : ApplicationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteBuilderApplicationBase"/> class.
        /// </summary>
        public SiteBuilderApplicationBase()
        {
            
            // Adds the httpmodule, if it's already added, this method does nothing
            WebConfigManager.AddHttpModule("USiteBuilderHttpModule", "Vega.USiteBuilder.USiteBuilderHttpModule, " + Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
