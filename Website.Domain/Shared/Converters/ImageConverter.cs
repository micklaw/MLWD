﻿using System;
using System.ComponentModel;
using System.Globalization;
using Umbraco.Core.Models;
using Website.Domain.Shared.Converters.Helpers;

namespace Website.Domain.Shared.Converters
{
    /// <summary>
    /// Provides a unified way of converting media picker properties to strong typed model.
    /// </summary>
    public class ImageConverter : TypeConverter
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
            if (sourceType == typeof(string) || sourceType == typeof(int))
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

            // DictionaryPublishedContent 
            var content = value as IPublishedContent;
            if (content != null)
            {
                // Use the id so we get folder sanitation.
                return BinderHelper.BindImage(content.Id);
            }

            if (value is int)
            {
                return BinderHelper.BindImage((int)value);
            }

            int id;
            var s = value as string;
            if (s != null && int.TryParse(s, out id))
            {
                return BinderHelper.BindImage(id);
            }

            return base.ConvertFrom(context, culture, value);
        }

        
    }
}