using Umbraco.Core.Models;

namespace Yomego.CMS.Core.Umbraco.Model
{
    public class CropImage : Image
    {
        public IPublishedContent MediaObject { get; set; }
    }
}
