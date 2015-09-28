﻿using System;
using System.Web.Mvc;

namespace Website.Domain.Shared.Views
{
    public abstract class WebsiteWebViewPage<T> : WebViewPage<T>
    {
        private readonly Lazy<WebsiteApp> _lazyApp = new Lazy<WebsiteApp>(() => new WebsiteApp());

        protected WebsiteApp DomainApp => _lazyApp.Value;

        public TimeSpan Subtract(DateTime date)
        {
            return DateTime.Now.Subtract(date);
        }

        public string Experience => Math.Floor((double)Subtract(new DateTime(2005, 1, 1)).Days / 365) + "+";

        public int Age => (int)Math.Floor((double)Subtract(new DateTime(1981, 5, 7)).Days / 365);
    }
}
