using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Website.Domain.Shared.Views
{
    public abstract class WebsiteWebViewPage<T> : WebViewPage<T>
    {
        public TimeSpan Subtract(DateTime date)
        {
            return DateTime.Now.Subtract(date);
        }

        public string Experience
        {
            get { return Math.Floor((double)Subtract(new DateTime(2005, 1, 1)).Days / 365) + "+"; }
        }

        public int Age
        {
            get { return (int)Math.Floor((double)Subtract(new DateTime(1981, 5, 7)).Days / 365); }
        }
    }
}
