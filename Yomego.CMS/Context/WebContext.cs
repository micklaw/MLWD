using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using Yomego.CMS.Caching;
using Yomego.CMS.Core.Caching;
using Yomego.CMS.Core.Caching.Interfaces;

namespace Yomego.CMS.Context
{
    public class WebContext
    {
        private CookieProvider _Cookies = new CookieProvider();

        private RequestProvider _Request = new RequestProvider();

        private SessionProvider _Session = new SessionProvider();

        private ICacheProvider _Cache = new CacheProvider();

        public virtual NameValueCollection QueryString
        {
            get
            {
                return HttpContext.Current.Request.QueryString;
            }
        }

        public virtual IDictionary Items
        {
            get
            {
                return HttpContext.Current.Items;
            }
        }

        public virtual IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        public virtual string DomainUrl
        {
            get
            {
               const string domainFormat = "{0}://{1}"; 
                
               return string.Format(domainFormat, HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host);
            }
        }

        public virtual Uri CurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.Url;
            }
        }

        public virtual SessionProvider Session 
        {
            get
            {
                return _Session;
            }
        }

        public virtual RequestProvider RequestCache
        {
            get
            {
                return _Request;
            }
        }

        public virtual CookieProvider Cookies
        {
            get
            {
                return _Cookies;
            }
        }

        public virtual ICacheProvider Cache
        {
            get
            {
                return _Cache;
            }
        }

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
