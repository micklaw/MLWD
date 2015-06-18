using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Website.Domain.Shared.Services;
using Website.Domain.Sitemap.Enums;
using Website.Domain.Sitemap.Models;
using Website.Domain.Sitemap.Models.Interfaces;

namespace Website.Domain.Sitemap.Services
{
    public class SitemapService : WebsiteService
    {
        private DateTime GetEntryLastModified(IPublishedContent entry)
        {
            if (entry.UpdateDate > entry.CreateDate)
                return entry.UpdateDate;

            return entry.CreateDate;
        }

        private void ListChildNodes(IPublishedContent parent, IList<ISitemapItem> list)
        {
            if (parent != null && parent.Id > 0)
            {
                if (!parent.GetPropertyValue<bool>("hidePage"))
                {
                    list.Add(new SitemapItem(parent.Url)
                    {
                        LastModified = GetEntryLastModified(parent),
                        Priority = 1,
                        ChangeFrequency = ChangeFrequency.Daily
                    });
                }

                var children = parent.Children;

                var publishedContents = children as IPublishedContent[] ?? children.ToArray();

                if (publishedContents.Any())
                {
                    foreach (var child in publishedContents)
                    {
                        ListChildNodes(child, list);
                    }
                }
            }
        }

        public IList<ISitemapItem> GetSitemapPages()
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);

            var pages = new List<ISitemapItem>();

            var rootNodes = helper.TypedContentAtRoot();

            var publishedContents = rootNodes as IPublishedContent[] ?? rootNodes.ToArray();

            if (publishedContents.Any())
            {
                foreach (var rootNode in publishedContents.Where(i => !i.DocumentTypeAlias.ToLower().Equals("settings")))
                {
                    ListChildNodes(rootNode, pages);
                }
            }

            return pages;
        }
    }
}
