using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string ForceLowered(this object input)
        {
            return input?.ToString().ToLower();
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
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
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
                text = Regex.Replace(text, "[" + splitChars + "]", "|||");
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

        public static string Base64Encode(string plainText)
        {
            if (plainText == null || plainText.Length % 4 != 0)
                plainText.PadLeft(16, '0');

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData == null || base64EncodedData.Length % 4 != 0)
                return "";

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
        /// </summary>
        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };

        /// <summary>
        /// Escapes a string according to the URI data string rules given in RFC 3986.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped value.</returns>
        /// <remarks>
        /// The <see cref="Uri.EscapeDataString"/> method is <i>supposed</i> to take on
        /// RFC 3986 behavior if certain elements are present in a .config file.  Even if this
        /// actually worked (which in my experiments it <i>doesn't</i>), we can't rely on every
        /// host actually having this configuration element present.
        /// </remarks>
        internal static string EscapeUriDataStringRfc3986(string value)
        {
            // Start with RFC 2396 escaping by calling the .NET method to do the work.
            // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
            // If it does, the escaping we do that follows it will be a no-op since the
            // characters we search for to replace can't possibly exist in the string.
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));

            // Upgrade the escaping to RFC 3986, if necessary.
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                escaped.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }
    }
}
