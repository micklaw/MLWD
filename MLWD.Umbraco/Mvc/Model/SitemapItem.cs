using System;
using MLWD.Umbraco.Mvc.Model.Enums;
using MLWD.Umbraco.Mvc.Model.Interfaces;

namespace MLWD.Umbraco.Mvc.Model
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