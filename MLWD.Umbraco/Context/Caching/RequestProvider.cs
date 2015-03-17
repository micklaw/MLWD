using System.Collections.Generic;
using System.Web;
using System.Collections;

namespace MLWD.Umbraco.Context.Caching
{
    /// <summary>
    /// Provides a wrapper for managing the RequestCache
    /// </summary>
    public class RequestProvider
    {
        private IDictionary _items { get; set; }

        public virtual IDictionary Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    if (_items == null)
                    {
                        _items = new Dictionary<string, object>();
                    }

                    return _items;
                }

                return HttpContext.Current.Items;
            }
        }

        /// <summary>
        /// Adds an item by its key to the Request Cache, updates if found
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Add(string key, object data)
        {
            if (!Current.Contains(key))
            {
                Current.Add(key, data);
            }
            else
            {
                Current["key"] = data;
            }

            return data;
        }

        /// <summary>
        /// Get an object form its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Current[key];
        }

        /// <summary>
        /// Gets and item form the cache by its key and tries to return it in its strong form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>Object expected or if null the default(T) response</returns>
        public T Get<T>(string key)
        {
            object item = Current[key];

            if (item == null || item.GetType() != typeof(T))
            {
                return default(T);
            }

            return (T)item;
        }

        /// <summary>
        /// Removes an item from the request items
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            Current.Remove(key);
        }
    }
}
