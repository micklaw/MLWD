using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Yomego.Umbraco.Mvc.Model.Media;
using Yomego.Umbraco.Umbraco.Helpers;

namespace Yomego.Umbraco.Umbraco.Ditto.TypeConverters
{
    /// <summary>
    /// Provides a unified way of converting multi media picker properties to strong typed collections.
    /// </summary>
    public class MultiImageConverter : TypeConverter
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
                return Enumerable.Empty<Image>();
            }

            // DictionaryPublishedContent 
            var content = value as IPublishedContent;

            if (content != null)
            {
                return BinderHelper.BindImage(content.Id);
            }

            // If a single item is selected, this is passed as an int, not a string.
            if (value is int)
            {
                var id = (int)value;
                return BinderHelper.BindImage(id);
            }

            var s = value as string;
            if (!string.IsNullOrWhiteSpace(s))
            {
                var multiNodeTreePicker = Enumerable.Empty<Image>();

                int n;
                var nodeIds =
                    XmlHelper.CouldItBeXml(s)
                    ? ConverterHelper.GetXmlIds(s)
                    : s
                        .ToDelimitedList()
                        .Select(x => int.TryParse(x, out n) ? n : -1)
                        .Where(x => x > 0)
                        .ToArray();

                if (nodeIds.Any())
                {
                    multiNodeTreePicker = nodeIds.Select(BinderHelper.BindImage).Where(i => i.Id > 0).AsEnumerable();
                }

                return multiNodeTreePicker;
            }

            return Enumerable.Empty<Image>();
        }
    }
}