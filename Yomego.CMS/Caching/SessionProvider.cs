using System.Web;
using System.Web.SessionState;

namespace Yomego.CMS.Caching
{
    public class SessionProvider
    {
        public virtual HttpSessionState Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.Session;
            }
        }

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Add(string key, object data)
        {
            if (Current != null)
            {
                Current.Add(key, data);
            }

            return data;
        }

        /// <summary>
        /// Gets an item from the cache by its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Current != null ? Current[key] : null;
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

            object item = Current[key];

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

        public void Clear()
        {
            Current.Clear();
        }
    }
}
