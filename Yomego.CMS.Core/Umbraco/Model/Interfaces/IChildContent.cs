using System.Collections.Generic;

namespace Yomego.CMS.Core.Umbraco.Model.Interfaces
{
    public interface IChildContent
    {
         IList<Content> Children { get; set; }
    }
}
