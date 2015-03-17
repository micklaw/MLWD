using System;
using MLWD.Umbraco.Mvc.Model.Enums;

namespace MLWD.Umbraco.Mvc.Model.Interfaces
{
    public interface ISitemapItem
    {
        string Url { get; }

        DateTime? LastModified { get; }

        ChangeFrequency? ChangeFrequency { get; }

        float? Priority { get; }
    }
}
