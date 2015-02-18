using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Core.Utils;

namespace Website.Domain.Shared.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString Image(this HtmlHelper html, string url, int width, string alt = "")
        {
            var noscript = new TagBuilder("noscript");

            noscript.Attributes.Add("lazy", "");
            noscript.Attributes.Add("data-actual", width.ToString());
            noscript.Attributes.Add("data-src", url);
            noscript.Attributes.Add("data-alt", alt);

            var image = new TagBuilder("img");

            image.Attributes.Add("lazy", "");
            image.AddCssClass("img-responsive");
            image.Attributes.Add("src", url);
            image.Attributes.Add("alt", alt);

            noscript.InnerHtml = image.ToString(TagRenderMode.SelfClosing);

            return new HtmlString(noscript.ToString());
        }

        public static IHtmlString Image(this HtmlHelper html, Image image)
        {
            if (image == null || !image.HasUrl)
            {
                return new HtmlString(string.Empty);
            }

            return html.Image(image.Url, image.Width, image.Alt);
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
