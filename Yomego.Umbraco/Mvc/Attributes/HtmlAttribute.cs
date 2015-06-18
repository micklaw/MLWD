using System;
using System.Web;

namespace Yomego.Umbraco.Mvc.Attributes
{
    public class HtmlAttribute : IHtmlString
    {
        private string _internalValue = String.Empty;
        private readonly string _seperator;

        public string Name { get; set; }
        public string Value { get; set; }
        public bool Condition { get; set; }

        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        public HtmlAttribute(string name, string seperator)
        {
            Name = name;

            _seperator = seperator ?? " ";
        }

        public HtmlAttribute Add(string value)
        {
            return Add(value, true);
        }

        public HtmlAttribute Add(string value, bool condition)
        {
            return Add(value, null, condition);
        }

        public HtmlAttribute Add(string passValue, string failValue, bool condition)
        {
            string value = condition ? passValue : failValue;

            if (!String.IsNullOrWhiteSpace(value))
            {
                _internalValue += value + _seperator;
            }

            return this;
        }

        public string ToHtmlString()
        {
            if (!String.IsNullOrWhiteSpace(_internalValue))
            {
                _internalValue = String.Format("{0}=\"{1}\"", Name,
                                               _internalValue.Substring(0, _internalValue.Length - _seperator.Length));
            }

            return _internalValue;
        }
    }
}
