﻿using Umbraco.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="T:System.Type"/> for inferring type properties.
    /// Most of this code was adapted from the Entity Framework
    /// </summary>
    public static class TypeInferenceExtensions
    {
        /// <summary>
        /// Determines whether the specified type is a collection type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is a collection type otherwise; false.</returns>
        public static bool IsCollectionType(this Type type)
        {
            return type.TryGetElementType(typeof(ICollection<>)) != null;
        }

        /// <summary>
        /// Determines whether the specified type is an enumerable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is an enumerable type otherwise; false.</returns>
        public static bool IsEnumerableType(this Type type)
        {
            return type.TryGetElementType(typeof(IEnumerable<>)) != null;
        }

        /// <summary>
        /// Determine if the given type implements the given generic interface or derives from the given generic type,
        /// and if so return the element type of the collection. If the type implements the generic interface several times
        /// <c>null</c> will be returned.
        /// </summary>
        /// <param name="type"> The type to examine. </param>
        /// <param name="interfaceOrBaseType"> The generic type to be queried for. </param>
        /// <returns> 
        /// <c>null</c> if <paramref name="interfaceOrBaseType"/> isn't implemented or implemented multiple times,
        /// otherwise the generic argument.
        /// </returns>
        public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
        {
            if (!type.IsGenericTypeDefinition)
            {
                Type[] types = GetGenericTypeImplementations(type, interfaceOrBaseType).ToArray();

                return types.Length == 1 ? types[0].GetGenericArguments().FirstOrDefault() : null;
            }

            return null;
        }

        /// <summary>
        /// Determine if the given type implements the given generic interface or derives from the given generic type,
        /// and if so return the concrete types implemented.
        /// </summary>
        /// <param name="type"> The type to examine. </param>
        /// <param name="interfaceOrBaseType"> The generic type to be queried for. </param>
        /// <returns> 
        /// The generic types constructed from <paramref name="interfaceOrBaseType"/> and implemented by <paramref name="type"/>.
        /// </returns>
        public static IEnumerable<Type> GetGenericTypeImplementations(this Type type, Type interfaceOrBaseType)
        {
            if (!type.IsGenericTypeDefinition)
            {
                return (interfaceOrBaseType.IsInterface ? type.GetInterfaces() : type.GetBaseTypes())
                        .Union(new[] { type })
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceOrBaseType);
            }

            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Gets the base types that the given type inherits from
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the base types from.</param>
        /// <returns>A collection of base types that the given type inherits from.</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            type = type.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }

        /// <summary>
        /// Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
        /// </summary>
        /// <param name="className">Name of the class sought.</param>
        /// <returns>Types that have the class name specified. They may not be in the same namespace.</returns>
        public static IEnumerable<Type> GetTypeByName<T>(string className)
        {
            var types = PluginManager.Current.ResolveTypes<T>();

            return types.Where(type => type.Name.ToLower() == className.ToLower());
        }
    }
}