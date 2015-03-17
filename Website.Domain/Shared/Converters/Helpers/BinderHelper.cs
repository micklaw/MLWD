using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLWD.Umbraco.Utils;
using Newtonsoft.Json;
using Website.Domain.Shared.Models;
using umbraco.MacroEngines;

namespace Website.Domain.Shared.Converters.Helpers
{
    public static class BinderHelper
    {
        public static Image BindImage(int mediaId)
        {
            return BindImage(mediaId.ToString());
        }

        public static Image BindImage(string mediaId)
        {
            var img = new Image();

            if (String.IsNullOrWhiteSpace(mediaId))
                return img;

            var mediaItem = new DynamicMedia(mediaId);

            if (mediaItem.Id > 0)
            {
                var mediaPath = mediaItem.GetPropertyAsString("umbracoFile");

                img.ImageCrops = JsonConvert.DeserializeObject<ImageCrops>(mediaPath);

                if (img.ImageCrops.focalPoint != null && !string.IsNullOrWhiteSpace(img.ImageCrops.src))
                {
                    img.Url = img.ImageCrops.src;
                }
                else
                {
                    img.Url = mediaPath;
                }

                int width;
                int.TryParse(mediaItem.GetPropertyAsString("umbracoWidth"), out width);
                img.Width = width;

                int height;
                int.TryParse(mediaItem.GetPropertyAsString("umbracoHeight"), out height);
                img.Height = height;

                int bytes;
                int.TryParse(mediaItem.GetPropertyAsString("umbracoBytes"), out bytes);
                img.Bytes = bytes;

                img.Alt = mediaItem.GetPropertyAsString("altTag");

                if (string.IsNullOrWhiteSpace(img.Alt))
                {
                    img.Alt = mediaItem.Name;
                }

                img.Title = mediaItem.GetPropertyAsString("titleTag");

                img.Id = mediaItem.Id;
                img.Name = mediaItem.Name;
            }

            return img;
        }
    }
}
