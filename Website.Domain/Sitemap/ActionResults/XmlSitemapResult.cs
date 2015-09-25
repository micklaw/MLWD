using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Website.Domain.Sitemap.Models.Interfaces;

namespace Website.Domain.Sitemap.ActionResults
{
    public class XmlSitemapResult : ActionResult
    {
        private readonly IEnumerable<ISitemapItem> _items;

        public XmlSitemapResult(IEnumerable<ISitemapItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var encoding = context.HttpContext.Response.ContentEncoding.WebName;

            var blank = XNamespace.Get(@"http://www.sitemaps.org/schemas/sitemap/0.9");

            var sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"),
                                        new XElement(blank + "urlset",
                                                     from item in _items
                                                     select CreateItemElement(blank, item)
                                            )
                );

            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }

        private XElement CreateItemElement(XNamespace blank, ISitemapItem item)
        {
            var url = new WebsiteApp().Context.DomainUrl;

            var itemElement = new XElement(blank + "url", new XElement(blank + "loc", url + item.Url.ToLower()));

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(blank + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(blank + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(blank + "priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

            return itemElement;
        }
    }
}