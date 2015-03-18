using System.Web;

namespace MLWD.Umbraco.Mvc.Model.Media
{
    public class Image : MediaItem
    {
        public string Alt { get; set; }

        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ImageCrops ImageCrops { get; set; }

        public string GetCrop(int? width = null, int? height = null)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            if (width.HasValue)
            {
                parameters["width"] = width.Value.ToString();
            }

            // [ML] - Having a fixed height is the only way we can use a focal point

            if (height.HasValue)
            {
                parameters["height"] = height.Value.ToString();

                if (ImageCrops != null && ImageCrops.focalPoint != null)
                {
                    parameters["center"] = ImageCrops.focalPoint.QueryString;
                }
            }

            if (width.HasValue || height.HasValue)
            {
                parameters["scale"] = "both";
                parameters["mode"] = "crop";
            }

            var queryString = parameters.ToString();

            return base.Url.Split('?')[0] + (!string.IsNullOrWhiteSpace(queryString) ? "?" + queryString : string.Empty);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
