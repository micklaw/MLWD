using System.Web;
using Umbraco.Core;
using Umbraco.Web;

namespace Yomego.Umbraco.Umbraco.Services.Content
{
    public class DictionaryService
    {
        private readonly UmbracoHelper _helper;

        public DictionaryService()
        {
            _helper = new UmbracoHelper(UmbracoContext.Current);
        }

        public string Get(string key)
        {
            var value = _helper.GetDictionaryValue(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                var service = ApplicationContext.Current.Services.LocalizationService;

                if (!service.DictionaryItemExists(key))
                {
                    service.CreateDictionaryItemWithIdentity(key, null, string.Empty);
                }
            }

            return value;
        }

        public IHtmlString GetAsHtml(string key)
        {
            return new HtmlString(key);
        }
    }
}
