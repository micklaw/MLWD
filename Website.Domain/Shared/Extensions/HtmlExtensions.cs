using System;
using System.Web;
using System.Web.Mvc;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Extensions;
using MLWD.Umbraco.Mvc.Model.Archetypes;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Utils;

namespace Website.Domain.Shared.Extensions
{
    public static class HtmlExtensions
    {
        public static HtmlAttribute NewWindow(this HtmlHelper html, UrlPicker urlPicker, bool value = false)
        {
            if (urlPicker != null)
            {
                value = urlPicker.NewWindow;
            }

            return html.Attr("target", value, "_blank");
        }

        public static string Url(this HtmlHelper html, UrlPicker urlPicker, string value = "javascript:void(0);")
        {
            if (urlPicker != null)
            {
                value = urlPicker.DisplayUrl;
            }

            return value;
        }

        public static IHtmlString FluidImage(this HtmlHelper html, Image image, int width, int? height = null, string alt = "", object htmlAttributes = null)
        {
            var noscript = new TagBuilder("noscript");

            if (image != null && image.HasUrl)
            {
                noscript.Attributes.Add("data-lazy-image", "");
                noscript.Attributes.Add("data-actual", width.ToString());
                noscript.Attributes.Add("data-actual-height", height.ToString());
                noscript.Attributes.Add("data-src", image.GetCrop(width, height));
                noscript.Attributes.Add("data-alt", alt);

                var imageTag = new TagBuilder("img");

                imageTag.Attributes.Add("data-lazy-image", "");
                imageTag.Attributes.Add("src", image.GetCrop(width, height));
                imageTag.Attributes.Add("alt", alt);

                if (htmlAttributes != null)
                {
                    noscript.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                    imageTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                }
                else
                {
                    noscript.AddCssClass("img-responsive");
                    imageTag.AddCssClass("img-responsive");
                }

                noscript.InnerHtml = imageTag.ToString(TagRenderMode.SelfClosing);
            }

            return new HtmlString(noscript.ToString());
        }

        public static IHtmlString FluidImage(this HtmlHelper html, Image image, int? width = null, int? height = null, object htmlAttributes = null)
        {
            if (image == null || !image.HasUrl)
            {
                return new HtmlString(string.Empty);
            }

            return html.FluidImage(image, width ?? image.Width, height ?? image.Height, image.Alt, htmlAttributes);
        }

        public static string ToDate(this HtmlHelper html, DateTime? date)
        {
            if (date.HasValue)
            {
                return string.Format("{0} {1}", date.Value.Day.ToOrdinal(), date.Value.ToString("MMMM yyyy"));
            }

            return null;
        }
    }
}
