using System;
using Yomego.CMS.Core.Enums;

namespace Yomego.CMS.Core.Mvc.Models.Interfaces
{
    public interface ISitemapItem
    {
        string Url { get; }

        DateTime? LastModified { get; }

        ChangeFrequency? ChangeFrequency { get; }

        float? Priority { get; }
    }
}
