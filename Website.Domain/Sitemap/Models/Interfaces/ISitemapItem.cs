using System;
using Website.Domain.Sitemap.Enums;

namespace Website.Domain.Sitemap.Models.Interfaces
{
    public interface ISitemapItem
    {
        string Url { get; }

        DateTime? LastModified { get; }

        ChangeFrequency? ChangeFrequency { get; }

        float? Priority { get; }
    }
}
