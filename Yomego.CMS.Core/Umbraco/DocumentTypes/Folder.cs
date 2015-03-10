using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Umbraco.DocumentTypes
{
    [ContentType(IconUrl = "icon-folder-outline", AllowedChildNodeTypes = new[] { typeof(Folder) })]
    public class Folder : Partial
    {
    }
}
