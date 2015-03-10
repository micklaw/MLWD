using System;
using System.Collections.Generic;

namespace Yomego.CMS.Core.Context
{
    public class Container
    {
        #region Resolution
        private static readonly IDictionary<Type, object> _implementations = new Dictionary<Type, object>();

        /// <summary>
        /// Resolve the requested type using an instance
        /// </summary>
        public static void ResolveUsing<TRequested>(object instance)
        {
            _implementations[typeof(TRequested)] = instance;
        }

        /// <summary>
        /// Resolve the requested type using an implementation type. The Implementation should
        /// have a parameterless constructor
        /// </summary>
        public static void ResolveUsing<TRequested, TImplementation>()
        {
            ResolveUsing<TRequested>(typeof(TImplementation));
        }

        /// <summary>
        /// Load a specified type
        /// </summary>
        public static T Load<T>() where T : class
        {
            if (_implementations.ContainsKey(typeof (T)))
            {
                var implementation = _implementations[typeof (T)];

                if (implementation is Type)
                {
                    return Activator.CreateInstance((Type) implementation) as T;
                }
                // Must be an instance
                return implementation as T;
            }

            if (typeof (T).IsAbstract)
            {
                // NOTE: Could be the content service
                throw new Exception("Please set a implementation to be used for the abstract type: " +
                                    typeof (T).Name + ", using the ResolveUsing methods");
            }

            return Activator.CreateInstance<T>();
        }


        #endregion

        private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        public T Get<T>() where T : class
        {
            if (!_instances.ContainsKey(typeof(T)))
            {
                _instances[typeof(T)] = Load<T>();
            }

            return _instances[typeof(T)] as T;
        }
    }
}