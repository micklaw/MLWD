﻿using Archetype.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using MLWD.Umbraco.Umbraco.ModelBuilder.Extensions;
using Umbraco.Core.Models;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a unified way of converting content picker properties to strong typed model.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of the object to return.
    /// </typeparam>
    public class ArchetypeConverter<T> : TypeConverter where T : class
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(ArchetypeModel).IsAssignableFrom(sourceType))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            IPublishedContent publishedContent = null;
            var publishedContext = context as PublishedContentContext;

            if (publishedContext != null)
            {
                publishedContent = publishedContext.Instance as IPublishedContent;
            }

            // DictionaryPublishedContent 
            var content = value as ArchetypeModel;

            if (content != null)
            {
                return content.As<T>(publishedContent, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}