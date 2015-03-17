using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using MLWD.Umbraco.Umbraco.ModelBuilder.Extensions;
using MLWD.Umbraco.Umbraco.ModelBuilder.Services.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Services.Concrete
{
    public class PropertyValueService : IPropertyValueService
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
        /// <param name="culture"></param>
        /// The <see cref="PropertyInfo"/> of the property to set on the object
        public void SetValue(Type type, object instance, object propertyValue, PropertyInfo propertyInfo, IPublishedContent content = null, CultureInfo culture = null)
        {
            if (propertyValue != null)
            {
                culture = culture ?? UmbracoContext.Current.PublishedContentRequest.Culture;

                var propertyType = propertyInfo.PropertyType;
                var typeInfo = propertyType.GetTypeInfo();
                var isEnumerableType = propertyType.IsEnumerableType() &&
                                       typeInfo.GenericTypeArguments.Any();

                // Try any custom type converters first.
                // 1: Check the property.
                // 2: Check any type arguments in generic enumerable types.
                // 3: Check the type itself.

                var converterAttribute =
                    propertyInfo.GetCustomAttribute<TypeConverterAttribute>()
                    ??
                    (isEnumerableType
                         ? CustomAttributeExtensions.GetCustomAttribute<TypeConverterAttribute>(typeInfo.GenericTypeArguments.First(), true)
                         : CustomAttributeExtensions.GetCustomAttribute<TypeConverterAttribute>(propertyType, true));

                if ((converterAttribute != null && converterAttribute.ConverterTypeName != null) && content != null)
                {
                    // Time custom conversions.
                    using (
                        DisposableTimer.DebugDuration(type,
                                                      string.Format("Custom TypeConverter ({0}, {1})", content.Id,
                                                                    propertyInfo.Name), "Complete"))
                    {
                        // Get the custom converter from the attribute and attempt to convert.

                        var toConvert = Type.GetType(converterAttribute.ConverterTypeName);

                        if (toConvert != null)
                        {
                            var converter = DependencyResolver.Current.GetService(toConvert) as TypeConverter;

                            if (converter != null && converter.CanConvertFrom(propertyValue.GetType()))
                            {
                                // Create context to pass to converter implementations.
                                // This contains the IPublishedContent and the currently converting property name.

                                var descriptor = TypeDescriptor.GetProperties(instance)[propertyInfo.Name];
                                var context = new PublishedContentContext(content, descriptor);
                                var converted = converter.ConvertFrom(context, culture, propertyValue);

                                if (converted != null)
                                {
                                    // Handle Typeconverters returning single objects when we want an IEnumerable.
                                    // Use case: Someone selects a folder of images rather than a single image with the media picker.

                                    if (isEnumerableType)
                                    {
                                        var parameterType = typeInfo.GenericTypeArguments.First();

                                        // Some converters return an IEnumerable so we check again.
                                        if (!converted.GetType().IsEnumerableType())
                                        {
                                            // Generate a method using 'Cast' to convert the type back to IEnumerable<T>.
                                            MethodInfo castMethod =
                                                typeof (Enumerable).GetMethod("Cast").MakeGenericMethod(parameterType);
                                            object enumerablePropertyValue = castMethod.Invoke(null,
                                                                                               new object[]
                                                                                                   {
                                                                                                       converted
                                                                                                   .YieldSingleItem()
                                                                                                   });
                                            propertyInfo.SetValue(instance, enumerablePropertyValue, null);
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(instance, converted, null);
                                        }
                                    }
                                    else
                                    {
                                        // Return single expected items from converters returning an IEnumerable.
                                        if (converted.GetType().IsEnumerableType())
                                        {
                                            // Generate a method using 'FirstOrDefault' to convert the type back to T.
                                            MethodInfo firstMethod = typeof (Enumerable)
                                                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                .First(
                                                    m =>
                                                        {
                                                            if (m.Name != "FirstOrDefault")
                                                            {
                                                                return false;
                                                            }

                                                            var parameters = m.GetParameters();
                                                            return parameters.Length == 1
                                                                   &&
                                                                   parameters[0].ParameterType.GetGenericTypeDefinition() ==
                                                                   typeof (IEnumerable<>);
                                                        })
                                                .MakeGenericMethod(propertyType);

                                            object singleValue = firstMethod.Invoke(null, new[] {converted});
                                            propertyInfo.SetValue(instance, singleValue, null);
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(instance, converted, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (propertyInfo.PropertyType == typeof (HtmlString) || propertyInfo.PropertyType == typeof (IHtmlString))
                {
                    // Handle Html strings so we don't have to set the attribute.
                    var converter = new HtmlStringConverter();

                    if (converter.CanConvertFrom(propertyValue.GetType()))
                    {
                        var descriptor = TypeDescriptor.GetProperties(instance)[propertyInfo.Name];
                        var context = new PublishedContentContext(content, descriptor);

                        propertyInfo.SetValue(instance, converter.ConvertFrom(context, culture, propertyValue), null);
                    }
                }
                else if (propertyInfo.PropertyType.IsInstanceOfType(propertyValue))
                {
                    // Simple types
                    propertyInfo.SetValue(instance, propertyValue, null);
                }
                else
                {
                    using (DisposableTimer.DebugDuration(type, string.Format("TypeConverter ({0}, {1})", content.Id, propertyInfo.Name), "Complete"))
                    {
                        var convert = propertyValue.TryConvertTo(propertyInfo.PropertyType);
                        if (convert.Success)
                        {
                            propertyInfo.SetValue(instance, convert.Result, null);
                        }
                    }
                }
            }
        }
    }
}
