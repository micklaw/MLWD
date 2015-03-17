using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Archetype.Models;
using MLWD.Umbraco.Umbraco.ModelBuilder.Services.Concrete;
using MLWD.Umbraco.Umbraco.ModelBuilder.Services.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Extensions
{
    public static class ArchetypeModelExtensions
    {
        /// <summary>
        /// The cache for storing constructor parameter information.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Type> _archetypeCache
            = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// The cache for storing type property information.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Add properties of type to the cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static void AddToPropertyCache(Type type)
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
        }

        /// <summary>
        /// Method used to convert ArchetypeModel to a stronly typed Model
        /// </summary>
        /// <param name="archetype">
        /// 
        /// </param>
        /// <param name="entityType">
        /// 
        /// </param>
        /// <param name="culture">
        /// 
        /// </param>
        /// <param name="content">
        /// 
        /// </param>
        /// <param name="propertyService">
        /// 
        /// </param>
        /// <returns></returns>
        private static object GetTypedArchetype(ArchetypeModel archetype, Type entityType, CultureInfo culture = null, IPublishedContent content = null, IPropertyValueService propertyService = null)
        {
            var isGenericList = entityType.IsGenericType && (entityType.GetGenericTypeDefinition() == typeof(IList<>) || entityType.GetGenericTypeDefinition() == typeof(List<>));

            var archetypeListType = isGenericList ? entityType.GetGenericArguments().FirstOrDefault() : entityType;

            if (archetypeListType == null)
            {
                throw new NullReferenceException(string.Format("The type ({0}) can not be inferred?", entityType.Name));
            }

            // ML - Build a generic list from the type fuond above

            var constructedListType = typeof(List<>).MakeGenericType(archetypeListType);
            var list = (IList)Activator.CreateInstance(constructedListType);

            if (archetype != null && (archetype.Fieldsets != null && archetype.Fieldsets.Any()))
            {
                if (archetype.Fieldsets != null && archetype.Fieldsets.Any())
                {
                    foreach (var fieldset in archetype.Fieldsets)
                    {
                        if (fieldset.Properties != null && fieldset.Properties.Any())
                        {
                            Type instanceType;

                            _archetypeCache.TryGetValue(fieldset.Alias, out instanceType);

                            if (instanceType == null)
                            {
                                // [ML] - find the first class which matches name and base type

                                instanceType = TypeInferenceExtensions.GetTypeByName<ArchetypeFieldsetModel>(fieldset.Alias).FirstOrDefault();

                                if (instanceType != null)
                                {
                                    _archetypeCache.TryAdd(fieldset.Alias, instanceType);

                                    AddToPropertyCache(instanceType);
                                }
                            }

                            if (instanceType != null)
                            {
                                // ML - Create an instance for each archetype object

                                var instance = Activator.CreateInstance(instanceType) as ArchetypeFieldsetModel;

                                if (instance != null)
                                {
                                    instance.Properties = fieldset.Properties;
                                    instance.Disabled = fieldset.Disabled;
                                    instance.Alias = fieldset.Alias;

                                    PropertyInfo[] properties;
                                    _propertyCache.TryGetValue(instanceType, out properties);

                                    if (properties != null && properties.Any())
                                    {
                                        foreach (var property in fieldset.Properties)
                                        {
                                            var propertyInfo = properties.FirstOrDefault(i => i.Name.ToLower() == property.Alias.ToLower());

                                            if (propertyInfo != null)
                                            {
                                                if (propertyService == null)
                                                {
                                                    propertyService = new PropertyValueService();
                                                }

                                                propertyService.SetValue(instanceType, instance, property.Value, propertyInfo, content, culture);
                                            }
                                        }

                                        // [ML] - If this is not a generic type, then return the first item

                                        if (!isGenericList)
                                        {
                                            return instance;
                                        }

                                        list.Add(instance);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return isGenericList ? list : null;
        }

        /// <summary>
        /// Returns the given instance of <see cref="ArchetypeModel"/> as the specified type.
        /// </summary>
        /// <param name="archetype">
        /// The <see cref="ArchetypeModel"/> to convert.
        /// </param>
        /// <param name="content">
        /// 
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>
        /// </param>
        /// <param name="propertyService">
        /// 
        /// </param>
        /// <typeparam name="T">
        /// The <see cref="Type"/> of items to return.
        /// </typeparam>
        /// <returns>
        /// The resolved <see cref="T"/>.
        /// </returns>
        public static T As<T>(
            this ArchetypeModel archetype,
            IPublishedContent content = null,
            CultureInfo culture = null,
            IPropertyValueService propertyService = null)
            where T : class
        {
            return As(archetype, typeof(T), culture, content, propertyService) as T;
        }


        /// <summary>
        /// Returns an object representing the given <see cref="Type"/>.
        /// </summary>
        /// <param name="archetype">
        /// The <see cref="ArchetypeModel"/> to convert.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of items to return.
        /// </param>
        /// <param name="content">
        /// 
        /// </param>
        /// <param name="culture">
        /// The <see cref="CultureInfo"/>
        /// </param>
        /// <param name="propertyService">
        /// 
        /// </param>
        /// <returns>
        /// The converted <see cref="Object"/> as the given type.
        /// </returns>
        public static object As(
            this ArchetypeModel archetype,
            Type type,
            CultureInfo culture = null,
            IPublishedContent content = null,
            IPropertyValueService propertyService = null)
        {
            if (archetype == null)
            {
                return null;
            }

            using (DisposableTimer.DebugDuration(type, string.Format("ArchetypeModel As ({0})", type.Name), "Complete"))
            {
                return GetTypedArchetype(archetype, type, culture, content, propertyService);
            }
        }
    }
}
