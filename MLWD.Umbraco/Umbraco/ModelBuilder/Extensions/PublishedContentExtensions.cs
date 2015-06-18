using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MLWD.Umbraco.Umbraco.ModelBuilder.Attributes;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel;
using MLWD.Umbraco.Umbraco.ModelBuilder.EventArgs;
using MLWD.Umbraco.Umbraco.ModelBuilder.Services.Concrete;
using MLWD.Umbraco.Umbraco.ModelBuilder.Services.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Extensions
{
    /// <summary>
    /// Encapsulates extension methods for <see cref="IPublishedContent"/>.
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// The cache for storing constructor parameter information.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ParameterInfo[]> _constructorCache
            = new ConcurrentDictionary<Type, ParameterInfo[]>();

        /// <summary>
        /// The cache for storing type property information.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Returns the given instance of <see cref="IPublishedContent"/> as the specified type.
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> to convert.
        /// </param>
        /// <param name="convertingType">
        /// The <see cref="Action{ConvertingTypeEventArgs}"/> to fire when converting.
        /// </param>
        /// <param name="convertedType">
        /// The <see cref="Action{ConvertedTypeEventArgs}"/> to fire when converted.
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="Type"/> of items to return.
        /// </typeparam>
        /// /// <param name="propertyService">
        /// The <see cref="IPropertyValueService"/> used for setting value based on types
        /// </param>
        /// <returns>
        /// The resolved <see cref="T"/>.
        /// </returns>
        public static T As<T>(
            this IPublishedContent content,
            Action<ConvertingTypeEventArgs> convertingType = null,
            Action<ConvertedTypeEventArgs> convertedType = null,
            CultureInfo culture = null,
            IPropertyValueService propertyService = null)
            where T : class
        {
            return content.As(typeof(T), convertingType, convertedType, culture, propertyService) as T;
        }

        /// <summary>
        /// Gets a collection of the given type from the given <see cref="IEnumerable{IPublishedContent}"/>.
        /// </summary>
        /// <param name="items">
        /// The <see cref="IEnumerable{IPublishedContent}"/> to convert.
        /// </param>
        /// <param name="documentTypeAlias">
        /// The document type alias.
        /// </param>
        /// <param name="convertingType">
        /// The <see cref="Action{ConvertingTypeEventArgs}"/> to fire when converting.
        /// </param>
        /// <param name="convertedType">
        /// The <see cref="Action{ConvertedTypeEventArgs}"/> to fire when converted.
        /// </param>
        /// <param name="culture">The <see cref="CultureInfo"/></param>
        /// <typeparam name="T">
        /// The <see cref="Type"/> of items to return.
        /// </typeparam>
        /// /// <param name="propertyService">
        /// The <see cref="IPropertyValueService"/> used for setting value based on types
        /// </param>
        /// <returns>
        /// The resolved <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<T> As<T>(
            this IEnumerable<IPublishedContent> items,
            string documentTypeAlias = null,
            Action<ConvertingTypeEventArgs> convertingType = null,
            Action<ConvertedTypeEventArgs> convertedType = null,
            CultureInfo culture = null,
            IPropertyValueService propertyService = null)
            where T : class
        {
            return items.As(typeof(T), documentTypeAlias, convertingType, convertedType, culture, propertyService)
                .Select(x => x as T);
        }

        /// <summary>
        /// Returns an object representing the given <see cref="Type"/>.
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> to convert.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of items to return.
        /// </param>
        /// <param name="convertingType">
        /// The <see cref="Action{ConvertingTypeEventArgs}"/> to fire when converting.
        /// </param>
        /// <param name="convertedType">
        /// The <see cref="Action{ConvertedTypeEventArgs}"/> to fire when converted.
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>
        /// </param>
        /// <param name="propertyService">
        /// The <see cref="IPropertyValueService"/> used for setting value based on types
        /// </param>
        /// <returns>
        /// The converted <see cref="Object"/> as the given type.
        /// </returns>
        public static object As(
            this IPublishedContent content,
            Type type,
            Action<ConvertingTypeEventArgs> convertingType = null,
            Action<ConvertedTypeEventArgs> convertedType = null,
            CultureInfo culture = null, 
            IPropertyValueService propertyService = null)
        {
            if (content == null)
            {
                return null;
            }

            using (DisposableTimer.DebugDuration(type, string.Format("IPublishedContent As ({0})", content.DocumentTypeAlias), "Complete"))
            {
                var convertingArgs = new ConvertingTypeEventArgs
                {
                    Content = content
                };

                EventHandlers.CallConvertingTypeHandler(convertingArgs);

                if (!convertingArgs.Cancel && convertingType != null)
                {
                    convertingType(convertingArgs);
                }

                // Cancel if applicable. 
                if (convertingArgs.Cancel)
                {
                    return null;
                }

                // Create an object and fetch it as the type.
                object instance = GetTypedProperty(content, type, culture, propertyService);

                // Fire the converted event
                var convertedArgs = new ConvertedTypeEventArgs
                {
                    Content = content,
                    Converted = instance,
                    ConvertedType = type
                };

                if (convertedType != null)
                {
                    convertedType(convertedArgs);
                }

                EventHandlers.CallConvertedTypeHandler(convertedArgs);

                return convertedArgs.Converted;
            }
        }

        /// <summary>
        /// Gets a collection of the given type from the given <see cref="IEnumerable{IPublishedContent}"/>.
        /// </summary>
        /// <param name="items">
        /// The <see cref="IEnumerable{IPublishedContent}"/> to convert.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of items to return.
        /// </param>
        /// <param name="documentTypeAlias">
        /// The document type alias.
        /// </param>
        /// <param name="convertingType">
        /// The <see cref="Action{ConvertingTypeEventArgs}"/> to fire when converting.
        /// </param>
        /// <param name="convertedType">
        /// The <see cref="Action{ConvertedTypeEventArgs}"/> to fire when converted.
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>.
        /// </param>
        /// /// <param name="propertyService">
        /// The <see cref="IPropertyValueService"/> used for setting value based on types
        /// </param>
        /// <returns>
        /// The resolved <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<object> As(
            this IEnumerable<IPublishedContent> items,
            Type type,
            string documentTypeAlias = null,
            Action<ConvertingTypeEventArgs> convertingType = null,
            Action<ConvertedTypeEventArgs> convertedType = null,
            CultureInfo culture = null,
            IPropertyValueService propertyService = null)
        {
            using (DisposableTimer.DebugDuration<IEnumerable<object>>(string.Format("IEnumerable As ({0})", documentTypeAlias)))
            {
                if (string.IsNullOrWhiteSpace(documentTypeAlias))
                {
                    return items.Select(x => x.As(type, convertingType, convertedType, culture, propertyService));
                }

                return items.Where(x => documentTypeAlias.InvariantEquals(x.DocumentTypeAlias))
                            .Select(x => x.As(type, convertingType, convertedType, culture, propertyService));
            }
        }

        /// <summary>
        /// Add properties of type to the cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> AddToPropertyCache(Type type)
        {
            PropertyInfo[] properties;
            _propertyCache.TryGetValue(type, out properties);

            if (properties == null)
            {
                properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.CanWrite)
                        .ToArray();

                _propertyCache.TryAdd(type, properties);
            }

            return properties;
        }

        
        /// <summary>
        /// Returns an object representing the given <see cref="Type"/>.
        /// </summary>
        /// <param name="content">
        /// The <see cref="IPublishedContent"/> to convert.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of items to return.
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>
        /// </param>
        /// <param name="propertyService">
        /// The <see cref="IPropertyValueService"/> used for setting value based on types
        /// </param>
        /// <returns>
        /// The converted <see cref="Object"/> as the given type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the given type has invalid constructors.
        /// </exception>
        private static object GetTypedProperty(IPublishedContent content, Type type, CultureInfo culture = null, IPropertyValueService propertyService = null)
        {
            // Check if the culture has been set, otherwise use from Umbraco.
            if (culture == null)
            {
                culture = ConverterHelper.GetCurrentCulture();
            }

            // Get the default constructor, parameters and create an instance of the type.
            // Try and return from the cache first. TryGetValue is faster than GetOrAdd.
            ParameterInfo[] constructorParams;
            _constructorCache.TryGetValue(type, out constructorParams);

            if (constructorParams == null)
            {
                var constructor = type.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault();

                if (constructor != null)
                {
                    constructorParams = constructor.GetParameters();
                    _constructorCache.TryAdd(type, constructorParams);
                }
            }

            object instance;
            if (constructorParams == null || constructorParams.Length == 0)
            {
                // Internally this uses Activator.CreateInstance which is heavily optimized.
                instance = type.GetInstance();
            }
            else if (constructorParams.Length == 1 & constructorParams[0].ParameterType == typeof(IPublishedContent))
            {
                // This extension method is about 7x faster than the native implementation.
                instance = type.GetInstance(content);
            }
            else
            {
                throw new InvalidOperationException("Type {0} has invalid constructor parameters");
            }

            // Collect all the properties of the given type and loop through writable ones.
            
            var properties = AddToPropertyCache(type);

            var contentType = content.GetType();

            foreach (var propertyInfo in properties)
            {
                using (DisposableTimer.DebugDuration(type, string.Format("ForEach Property ({1} {0})", propertyInfo.Name, content.Id), "Complete"))
                {
                    // Check for the ignore attribute.
                    var ignoreAttr = propertyInfo.GetCustomAttribute<DittoIgnoreAttribute>();
                    if (ignoreAttr != null)
                    {
                        continue;
                    }

                    var umbracoPropertyName = propertyInfo.Name;
                    var altUmbracoPropertyName = string.Empty;
                    var recursive = false;
                    object defaultValue = null;

                    var umbracoPropertyAttr = propertyInfo.GetCustomAttribute<UmbracoPropertyAttribute>();

                    if (umbracoPropertyAttr != null)
                    {
                        umbracoPropertyName = umbracoPropertyAttr.PropertyName;
                        altUmbracoPropertyName = umbracoPropertyAttr.AltPropertyName;
                        recursive = umbracoPropertyAttr.Recursive;
                        defaultValue = umbracoPropertyAttr.DefaultValue;
                    }

                    // Try fetching the value.
                    var contentProperty = contentType.GetProperty(umbracoPropertyName);
                    object propertyValue = contentProperty != null ? contentProperty.GetValue(content, null) : content.GetPropertyValue(umbracoPropertyName, recursive);

                    // Try fetching the alt value.
                    if ((propertyValue == null || propertyValue.ToString().IsNullOrWhiteSpace()) && !string.IsNullOrWhiteSpace(altUmbracoPropertyName))
                    {
                        contentProperty = contentType.GetProperty(altUmbracoPropertyName);

                        propertyValue = contentProperty != null ? contentProperty.GetValue(content, null) : content.GetPropertyValue(altUmbracoPropertyName, recursive);
                    }

                    // Try setting the default value.
                    if ((propertyValue == null || propertyValue.ToString().IsNullOrWhiteSpace()) && defaultValue != null)
                    {
                        propertyValue = defaultValue;
                    }

                    // [ML] - If we dont have a propertyService as override, then use the Ditto default for this POCO
                    if (propertyService == null)
                    {
                        propertyService = new PropertyValueService();    
                    }

                    propertyService.SetValue(type, instance, propertyValue, propertyInfo, content, culture);
                }
            }

            return instance;
        }
    }
}