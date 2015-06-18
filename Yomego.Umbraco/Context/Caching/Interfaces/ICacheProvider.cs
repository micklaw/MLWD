using System;

namespace Yomego.Umbraco.Context.Caching.Interfaces
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiryDate"></param>
        /// <returns></returns>
        object Add(string key, object data, DateTime expiryDate);

        /// <summary>
        /// Gets an item from the cache by its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Gets and item form the cache by its key and tries to return it in its strong form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>Object expected or if null the default(T) response</returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes and item fromr the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Clear all cache items
        /// </summary>
        void Clear();
    }
}