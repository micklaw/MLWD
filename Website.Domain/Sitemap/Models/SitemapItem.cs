using System;
using Website.Domain.Sitemap.Enums;
using Website.Domain.Sitemap.Models.Interfaces;

namespace Website.Domain.Sitemap.Models
{
    public class SitemapItem : ISitemapItem
    {
        public SitemapItem(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public float? Priority { get; set; }
    }
}