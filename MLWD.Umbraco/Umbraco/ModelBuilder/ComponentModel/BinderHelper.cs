using System;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Utils;
using Newtonsoft.Json;
using umbraco.MacroEngines;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel
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

                img.Url = !string.IsNullOrWhiteSpace(img.ImageCrops.src) ? img.ImageCrops.src : mediaPath;

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
