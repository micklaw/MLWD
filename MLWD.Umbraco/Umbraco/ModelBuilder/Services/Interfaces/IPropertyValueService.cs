using System;
using System.Globalization;
using System.Reflection;
using Umbraco.Core.Models;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Services.Interfaces
{
    public interface IPropertyValueService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">
        ///     The <see cref="Type"/> to convert too.
        /// </param>
        /// <param name="instance">
        ///     The <see cref="object"/> to populate.
        /// </param>
        /// <param name="propertyValue">
        ///     The <see cref="object"/> whos property we will populate with.
        /// </param>
        /// <param name="propertyInfo"></param>
        /// <param name="content">
        ///     The <see cref="IPublishedContent"/> of which to use as a base for populating content
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> for populating the content
        /// </param>
        /// The <see cref="PropertyInfo"/> of the property to set on the object
        void SetValue(Type type, object instance, object propertyValue, PropertyInfo propertyInfo, IPublishedContent content = null, CultureInfo culture = null);
    }
}