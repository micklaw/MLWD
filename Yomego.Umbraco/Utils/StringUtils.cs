using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Humanizer;
using Yomego.Umbraco.Mvc.Model.Media;

namespace Yomego.Umbraco.Utils
{
    public static class StringUtils
    {
        public static string GetCrop(this string url, ImageCrops crops, int? width = null, int? height = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            var parameters = HttpUtility.ParseQueryString(string.Empty);

            if (width.HasValue)
            {
                parameters["width"] = width.Value.ToString();
            }

            // [ML] - Having a fixed height is the only way we can use a focal point

            if (height.HasValue)
            {
                parameters["height"] = height.Value.ToString();

                if (crops != null && crops.focalPoint != null)
                {
                    parameters["center"] = crops.focalPoint.QueryString;
                }
            }

            if (width.HasValue || height.HasValue)
            {
                parameters["scale"] = "both";
                parameters["mode"] = "crop";
            }

            var queryString = parameters.ToString();

            return url + (!string.IsNullOrWhiteSpace(queryString) ? "?" + queryString : string.Empty);
        }

        public static string GetImageGenThumbnail(this string imageUrl, int? width, int? height, int? compression = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return null;
            }

            var url = "/imagegen.ashx?image=" + imageUrl;

            if (width > 0)
                url += String.Format("&width={0}", width);

            if (height > 0)
                url += String.Format("&height={0}", height);

            if (compression.HasValue)
                url += String.Format("&compression={0}", compression.Value);

            return url;
        }

        /// <summary>
        /// Generate a url slug from a string
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                return phrase;
            }

            string slug = phrase.RemoveAccent().ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens  

            return slug;
        }


        /// <summary>
        /// Remove any accent characters form the url
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Add parameter to URL string
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddParameterToUrl(string url, string key, string value)
        {
            if (url != null)
            {
                var q = new UrlUtils(url);
                q.Set(key, value);
                return q.AbsoluteUri;
            }

            return url;
        }

        public static string RemoveTags(this IHtmlString sXML)
        {
            if (sXML == null)
            {
                return null;
            }

            var xml = sXML.ToString();

            return RemoveTags(xml);
        }

        public static string RemoveTags(this string sXML)
        {
            return Regex.Replace(sXML, "<(.|\n)+?>", String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>NOTE: Returns an empty array if text is null</returns>
        public static string[] SplitOnCommonChars(string text)
        {
            return SplitOnChars(text, ";,|");
        }

        public static string[] SplitOnChars(string text, string splitChars)
        {
            string[] splits = null;
            if (text != null)
            {
                text = text.Trim();
                text = System.Text.RegularExpressions.Regex.Replace(text, "[" + splitChars + "]", "|||");
                splits = text.Split(new string[1] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                splits = new string[0];
            }
            return splits;
        }

        public static string RemoveTrailingComma(string s)
        {
            return RemoveTrailingDelimter(s, ",");
        }

        public static string RemoveTrailingDelimter(string s, string delimeter)
        {
            if (s == null)
                return null;

            if (s.EndsWith(delimeter))
                s = s.Substring(0, s.Length - 1);

            return s;
        }

        public static string RemoveTrailingSlash(string s)
        {
            return RemoveTrailingDelimter(s, "/");
        }

        public static string RemoveLeadingSlash(string s)
        {
            if (s == null)
                return null;

            if (s.StartsWith("/"))
                s = s.Substring(1);

            return s;
        }

        public static IList<string> SplitOnNewLines(string source)
        {
            return Regex.Split(source, @"\r?\n").Where(d => !String.IsNullOrWhiteSpace(d)).ToList();
        }

        public static string ConcatenateList(IEnumerable list, string delimeter)
        {
            string concatenated = null;

            if (list != null)
            {
                concatenated = String.Empty;

                foreach (var item in list)
                {
                    if (item != null)
                    {
                        concatenated += item.ToString() + delimeter;
                    }
                }

                concatenated = RemoveTrailingDelimter(concatenated, delimeter);
            }

            return concatenated;
        }

        public static string ConcatenateListUsingCommas(IEnumerable list)
        {
            return ConcatenateList(list, ",");
        }

        public static string ConcatenateListUsingNewLines(IEnumerable list)
        {
            return ConcatenateList(list, Environment.NewLine);
        }

        public static string StatusFormat(string rawText)
        {
            Regex UrlReplacement = new Regex(@"(https?://([\w-]+\.)+[\w-]+([^ ]*))", RegexOptions.Compiled);
            Regex UserNameReplacement = new Regex(@"@([\w_]+)", RegexOptions.Compiled);
            Regex HashTagReplacement = new Regex("(#)((?:[A-Za-z0-9-_]*))");

            string formattedText = string.Empty;

            formattedText = UrlReplacement.Replace(rawText, "<a href=\"$1\">$1</a>");
            formattedText = UserNameReplacement.Replace(formattedText, "@<a href=\"http://twitter.com/$1\">$1</a>");
            formattedText = HashTagReplacement.Replace(formattedText, "#<a href=\"http://search.twitter.com/search?q=$1\">$1</a>");

            return formattedText;

        }

        public static string ReplaceNewLinesWithBR(string source)
        {
            return ReplaceNewLines(source, "<br/>");
        }

        public static string ReplaceNewLines(string source, string replace)
        {
            return Regex.Replace(source, @"\r?\n", replace);

        }

        public static float? StringToFloat(string value)
        {
            return StringToFloat(value, null);
        }

        public static float? StringToFloat(string value, float? defaultValue)
        {
            try
            {
                return float.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal? StringToDecimal(string value)
        {
            return StringToDecimal(value, null);
        }

        public static decimal? StringToDecimal(string value, decimal? defaultValue)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int? StringToInt(string value)
        {
            return StringToInt(value, null);
        }

        public static int? StringToInt(string value, int? defaultValue)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string CleanTextForLucene(string value)
        {
            if (value == null)
                value = String.Empty;

            Regex regex = new Regex("[^a-zA-Z0-9 ]");

            string replacedValue = regex.Replace(value, " ");

            return replacedValue;
        }

        public static string UnTokenize(this string input, char delimiter = ',', string tokenChar = "_")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return string.Join(" ", input.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Replace(" ", tokenChar)));
        }

        public static string FixUnTokenized(this string input, string tokenChar = "_", LetterCasing casing = LetterCasing.Title)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return input.Replace(" ", "_").Humanize(casing);
        }
    }
}
