using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Content = Yomego.CMS.Core.Umbraco.Model.Content;

namespace Vega.USiteBuilder
{
    public static class ContentHelperExtensions
    {
        public static IEnumerable<TType> As<TType>(this IEnumerable<IPublishedContent> nodes)
            where TType : Content, new()
        {
            return nodes.Select(n => ContentHelper.GetByNodeId<TType>(n.Id));
        }
    }
}