using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.Templates;
using System.Collections.Specialized;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Collections;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Core.Utils;

namespace Yomego.CMS.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        #region Conditional Html Attribute Helpers

        public static HtmlAttribute Css(this HtmlHelper html, string value)
        {
            return Css(html, value, true);
        }

        public static HtmlAttribute Css(this HtmlHelper html, string value, bool condition)
        {
            return Css(html, null, value, condition);
        }

        public static HtmlAttribute Css(this HtmlHelper html, string seperator, string value, bool condition)
        {
            return new HtmlAttribute("class", seperator).Add(value, condition);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string value)
        {
            return Attr(html, name, true, value);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, bool condition, string value)
        {
            return Attr(html, name, null, condition, value);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, bool condition, string value, string failValue)
        {
            return Attr(html, name, null, condition, value, failValue);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string seperator, bool condition, string value)
        {
            return new HtmlAttribute(name, seperator).Add(value, condition);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string seperator, bool condition, string value, string failValue)
        {
            return new HtmlAttribute(name, seperator).Add(value, failValue, condition);
        }

        #endregion Conditional Html Attribute Helpers

        #region Paging Extensions

        public static HtmlString RenderPager<T>(this HtmlHelper html, IPagedList<T> list)
        {
            return RenderPager(html, list, new { });
        }

        /// <summary>
        /// Helper to simply add a "p" param onto the end of the current url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static HtmlString RenderPager<T>(this HtmlHelper html, IPagedList<T> list, object htmlAttributes)
        {
            Uri currentUri = HttpContext.Current.Request.Url;

            // Ensure the query string doesn't already have a "page" param
            NameValueCollection qs = HttpUtility.ParseQueryString(currentUri.Query);
            qs.Remove("p");

            // Build our new URL
            UriBuilder builder = new UriBuilder(currentUri);
            builder.Query = qs.ToString();
            string url = builder.ToString();

            return RenderPager(html, list, i => StringUtils.AddParameterToUrl(url, "p", i.ToString()), htmlAttributes);
        }

        public static HtmlString RenderPager<T>(this HtmlHelper html, IPagedList<T> list, Func<int, string> pageUrl)
        {
            return RenderPager(html, list, pageUrl, null);
        }

        private static void RenderPageInfo(StringBuilder result, int page, int currentPage, int totalPages, string text, Func<int, string> pageUrl, string css)
        {
            var isCurrent = (css != null);

            var li = new TagBuilder("li");

            if (isCurrent)
                li.AddCssClass(css);

            // Render the opening LI tag
            result.AppendLine(li.ToString(TagRenderMode.StartTag));

            var a = new TagBuilder(isCurrent ? "span" : "a");

            if (!isCurrent)
            {
                a.MergeAttribute("href", pageUrl(page));
            }

            a.InnerHtml = text;
            result.AppendLine(a.ToString(TagRenderMode.Normal));

            // Render the closing LI tag
            result.AppendLine(li.ToString(TagRenderMode.EndTag));
        }

        public static HtmlString RenderPager<T>(this HtmlHelper html, IPagedList<T> list, Func<int, string> pageUrl, object htmlAttributes)
        {
            // Hide the pager if there are no results
            if (list.TotalPages < 2)
                return new HtmlString(String.Empty);


            StringBuilder result = new StringBuilder();

            // Wrapping ul
            TagBuilder ul = new TagBuilder("ul");
            ul.MergeAttributes<string, object>(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            result.AppendLine(ul.ToString(TagRenderMode.StartTag));

            int startingPosition = 1;
            int currentPage = list.PageIndex;

            // Open li
            //RenderPageInfo(result, Math.Max(1, currentPage - 1), currentPage, list.TotalPages, "Prev", pageUrl, currentPage == 1 ? "active" : null);

            // Calculate starting position
            if (currentPage > 4)
                startingPosition = currentPage - 3;

            for (int i = startingPosition; i <= Math.Min(startingPosition + 5, list.TotalPages); i++)
                RenderPageInfo(result, i, currentPage, list.TotalPages, i.ToString(), pageUrl, i == currentPage ? "active" : null);

            //RenderPageInfo(result, Math.Min(currentPage + 1, list.TotalPages), currentPage, list.TotalPages, "Next", pageUrl, currentPage == list.TotalPages ? "active" : null);

            // Render the closing UL
            result.AppendLine(ul.ToString(TagRenderMode.EndTag));

            return new HtmlString(result.ToString());
        }
        #endregion

        #region Umbraco

        public static IHtmlString RenderMacro(this HtmlHelper html, string alias, object htmlAttributes)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);

            return helper.RenderMacro(alias, htmlAttributes);
        }

        public static IHtmlString Dictionary(this HtmlHelper html, string key)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);

            return new HtmlString(helper.GetDictionaryValue(key) ?? String.Empty);
        }

        public static IHtmlString RawUmbraco(this HtmlHelper html, IHtmlString body)
        {
            if (body == null)
            {
                return null;
            }

            return RawUmbraco(html, body.ToString());
        }

        public static IHtmlString RawUmbraco(this HtmlHelper html, string body)
        {
            if (String.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            string newBody = TemplateUtilities.ParseInternalLinks(body);

            return new HtmlString(newBody);
        }

        #endregion Umbraco

        public static HtmlAttribute GradientBackground(this HtmlHelper html, string topColour, string fadeColour, string textcolour = null)
        {
            string style = "background: {0}; " +
                           "background: -moz-radial-gradient(center, ellipse cover, {0} 0%, {1} 100%); " +
                           "background: -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(0%, {0}), color-stop(100%, {1})); " +
                           "background: -webkit-radial-gradient(center, ellipse cover, {0} 0%, {1} 100%); " +
                           "background: -o-radial-gradient(center, ellipse cover, {0} 0%, {1} 100%); " +
                           "background: -ms-radial-gradient(center, ellipse cover, {0} 0%, {1} 100%); " +
                           "background: radial-gradient(ellipse at center, {0} 0%, {1} 100%); " +
                           "filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='{0}', endColorstr='{1}',GradientType=1 );";

            if (!String.IsNullOrWhiteSpace(textcolour))
            {
                style += " color: {2};";
            }

            var hasValue = (!String.IsNullOrWhiteSpace(topColour) && !String.IsNullOrWhiteSpace(fadeColour));

            return Attr(html, "style", hasValue, String.Format(style, "#" + topColour, "#" + fadeColour, "#" + textcolour));
        }

        public static IHtmlString AddAttributes(this HtmlHelper html, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) as IDictionary<string, object>;

            var attributeString = String.Empty;

            if (attributes != null && attributes.Count > 0)
            {
                attributeString = String.Join(" ", attributes.Select(i => String.Format("{0}=\"{1}\"", i.Key.Replace("-", "_"), i.Value.ToString().Replace("\"", "'"))));
            }

            return new HtmlString(attributeString);
        }

        public static HtmlAttribute IsCurrentPage(this HtmlHelper html, Content content)
        {
            var url = HttpContext.Current.Request.RawUrl;

            var isSelected = (url.Trim('/').ToLower().StartsWith(content.Url.ToLower().Trim('/')));

            const string className = "active";

            return html.Attr("class", isSelected, className);
        }

        public static HtmlAttribute AreEqual(this HtmlHelper html, string value, object postValue)
        {
            if (String.IsNullOrWhiteSpace(value) || postValue == null)
            {
                return null;
            }

            return html.Attr("selected", (value.ToLower() == postValue.ToString().ToLower()), "selected");
        }

        public static IHtmlString FluidImage(this HtmlHelper html, Image image, int width, int? height = null, string alt = "", object htmlAttributes = null)
        {
            var noscript = new TagBuilder("noscript");

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
    }
}
