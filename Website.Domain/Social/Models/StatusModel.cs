using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Website.Domain.Social.Models
{
    public class StatusModel
    {
        public string FormatTweet(string rawText)
        {
            var urlReplacement = new Regex(@"(https?://([\w-]+\.)+[\w-]+([^ ]*))", RegexOptions.Compiled);
            var userNameReplacement = new Regex(@"@([\w_]+)", RegexOptions.Compiled);

            string formattedText = urlReplacement.Replace(rawText, "<a target=\"_blank\" href=\"$1\">$1</a>");
            formattedText = userNameReplacement.Replace(formattedText, "<a target=\"_blank\" href=\"http://twitter.com/$1\">@$1</a>");

            return formattedText;
        }

        public ulong Id { get; set; }

        public string Text { get; set; }

        public HtmlString TextAsHtml => new HtmlString(FormatTweet(Text ?? string.Empty));

        public string Handle { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }
    }
}