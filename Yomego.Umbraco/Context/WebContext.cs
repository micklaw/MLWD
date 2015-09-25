using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using Yomego.Umbraco.Context.Caching;
using Yomego.Umbraco.Context.Caching.Interfaces;

namespace Yomego.Umbraco.Context
{
    public class WebContext
    {
        public virtual NameValueCollection QueryString => HttpContext.Current.Request.QueryString;

        public virtual IDictionary Items => HttpContext.Current.Items;

        public virtual IPrincipal User => HttpContext.Current.User;


        public virtual string DomainUrl
        {
            get
            {
               const string domainFormat = "{0}://{1}"; 
                
               return string.Format(domainFormat, HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host);
            }
        }

        public virtual Uri CurrentUrl => HttpContext.Current.Request.Url;

        public virtual SessionProvider Session { get; } = new SessionProvider();

        public virtual RequestProvider RequestCache { get; } = new RequestProvider();

        public virtual CookieProvider Cookies { get; } = new CookieProvider();

        public virtual ICacheProvider Cache { get; } = new CacheProvider();

        public string CurrentCulture
        {
            get
            {
                if(Items["CurrentCulture"] == null)
                    Items["CurrentCulture"] = new App().Services.Content.GetCurrentCulture();

                return Items["CurrentCulture"] as string;
            }
        }
    }
}
