using System;
using System.Web;
using System.Web.Caching;
using Yomego.Umbraco.Context.Caching.Interfaces;

namespace Yomego.Umbraco.Context.Caching
{
    public class CacheProvider : ICacheProvider
    {
        private Cache Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiryDate"></param>
        /// <returns></returns>
        public object Add(string key, object data, DateTime expiryDate)
        {
            Current?.Insert(key, data, null, expiryDate, Cache.NoSlidingExpiration);

            return data;
        }

        /// <summary>
        /// Gets an item from the cache by its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Current?.Get(key);
        }

        /// <summary>
        /// Gets and item form the cache by its key and tries to return it in its strong form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>Object expected or if null the default(T) response</returns>
        public T Get<T>(string key)
        {
            if (Current == null)
            {
                return default(T);
            }

            var item = Current.Get(key);

            if (item == null || item.GetType() != typeof(T))
            {
                return default(T);
            }

            return (T)item;
        }

        /// <summary>
        /// Removes and item fromr the cache
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (Current[key] != null)
            {
                Current.Remove(key);
            }
        }
    }
}
