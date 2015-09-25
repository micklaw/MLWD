using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Our.Umbraco.Ditto;
using Umbraco.Core.Dynamics;
using Umbraco.Web.Templates;

namespace Yomego.Umbraco.Umbraco.Ditto.TypeConverters
{
    /// <summary>
    /// Provides a unified way of converting <see cref="String"/>s to <see cref="HtmlString"/>'s.
    /// </summary>
    public class HtmlStringConverter : DittoConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter,
        /// using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.
        /// </param>
        /// <param name="sourceType">
        /// A <see cref="T:System.Type" /> that represents the type you want to convert from.
        /// </param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // We can pass null here.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (sourceType == null
                || sourceType == typeof(string)
                || sourceType == typeof(HtmlString)
                || sourceType == typeof(DynamicXml))
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

            if (value is string || value is HtmlString)
            {
                var text = value.ToString();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    text = Regex.Replace(text, @"\t|\n|\r", "");
                    text = TemplateUtilities.ParseInternalLinks(text);
                }

                return new HtmlString(text);
            }

            if (value is DynamicXml)
            {
                return ((DynamicXml)value).ToHtml();
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}