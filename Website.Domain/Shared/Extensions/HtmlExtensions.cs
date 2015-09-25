using System.Web.Mvc;
using Website.Domain.Shared.Archetypes;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Extensions;

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

    }
}
