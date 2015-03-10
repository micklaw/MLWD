using System;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Mvc.Models.Interfaces;

namespace Yomego.CMS.Core.Mvc.Models
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