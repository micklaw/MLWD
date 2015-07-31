using System.Web;

namespace Yomego.Umbraco.Mvc.Model.Media
{
    public class Image : MediaItem
    {
        public string Alt { get; set; }

        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ImageCrops ImageCrops { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
