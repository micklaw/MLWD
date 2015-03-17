using System.ComponentModel;
using Umbraco.Core.Models;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.EventArgs
{
    /// <summary>
    /// Provides data for a converting event.
    /// </summary>
    public class ConvertingTypeEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public IPublishedContent Content { get; set; }
    }
}