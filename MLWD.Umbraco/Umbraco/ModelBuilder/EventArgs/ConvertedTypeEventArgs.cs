using System;
using Umbraco.Core.Models;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.EventArgs
{
    /// <summary>
    /// Provides data for a converted event.
    /// </summary>
    public class ConvertedTypeEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public IPublishedContent Content { get; set; }

        /// <summary>
        /// Gets or sets the converted object.
        /// </summary>
        public object Converted { get; set; }

        /// <summary>
        /// Gets or sets the converted type.
        /// </summary>
        public Type ConvertedType { get; set; }
    }
}