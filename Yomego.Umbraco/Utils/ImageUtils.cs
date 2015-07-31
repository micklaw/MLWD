using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Yomego.Umbraco.Mvc.Model.Media;

namespace Yomego.Umbraco.Utils
{
    public static class ImageUtils
    {
        public static string GetCrop(this Image image, ImageCrops crops, int? width = null, int? height = null)
        {
            if (image == null || !image.HasUrl)
            {
                return "about:blank";
            }

            return image.Url.GetCrop(crops, width, height);
        }

        public static string GetCrop(this Image image, int? width = null, int? height = null)
        {
            return image.GetCrop(image.ImageCrops, width, height);
        }
    }
}
