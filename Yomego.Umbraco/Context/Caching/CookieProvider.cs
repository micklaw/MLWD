using System;
using System.Web;
using Newtonsoft.Json;

namespace Yomego.Umbraco.Context.Caching
{
    public class CookieProvider
    {
        /// <summary>
        /// Provides a wrapper for managing Cookies
        /// </summary>
        public virtual HttpCookieCollection Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new Exception("Cookie Provider can only be used in a web context");
                }

                if (HttpContext.Current.Response == null)
                {
                    throw new Exception("Respons cannot be null while requesting cookies");
                }

                return HttpContext.Current.Response.Cookies;
            }
        }

        /// <summary>
        /// Adds in item to the cookie collection
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expires">Leave null to expire at the end of the session</param>
        /// <returns></returns>
        public object Add(string key, object data, DateTime? expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null)
            {
                cookie = new HttpCookie(key);
            }

            if (expires != null)
            {
                cookie.Expires = expires.Value;
            }

            cookie.HttpOnly = true;
            cookie.Value = JsonConvert.SerializeObject(data);

            Current.Add(cookie);

            return data;
        }

        /// <summary>
        /// Gets and item from the request items by its key and tries to return it in its strong form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>Object expected or if null the default(T) response</returns>
        public T Get<T>(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null)
            {
                return default(T);
            }

            return (T)JsonConvert.DeserializeObject<T>(cookie.Value);
        }

        /// <summary>
        /// Removes an item from the cookie collection
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = string.Empty;

                Current.Add(cookie);
            }
        }
    }
}
