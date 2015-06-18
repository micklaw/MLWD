using System;
using CookComputing.MetaWeblog;
using Newtonsoft.Json;
using umbraco.MacroEngines;
using Yomego.Umbraco.Mvc.Model.Media;
using Yomego.Umbraco.Utils;

namespace Yomego.Umbraco.Umbraco.Helpers
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

                img.ImageCrops = mediaPath != null ? JsonConvert.DeserializeObject<ImageCrops>(mediaPath) : new ImageCrops();

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

        public static Asset BindAsset(int mediaId)
        {
            return BindAsset(mediaId.ToString());
        }

        public static Asset BindAsset(string mediaId)
        {
            var img = new Asset();

            if (String.IsNullOrWhiteSpace(mediaId))
                return img;

            var mediaItem = new DynamicMedia(mediaId);

            if (mediaItem.Id > 0)
            {
                img.Url = mediaItem.GetPropertyAsString("umbracoFile");
                img.Id = mediaItem.Id;
                img.Name = mediaItem.Name;
            }

            return img;
        }
    }
}
