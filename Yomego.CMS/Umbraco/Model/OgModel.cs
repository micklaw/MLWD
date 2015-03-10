using System.Web;

namespace Yomego.CMS.Core.Umbraco.Model
{
    public class OgModel
    {
        private string _linkFormat = "{0}://{1}{2}";

        public OgModel() : this("food")
        {

        }

        public OgModel(string pageType)
        {
            _linkFormat = string.Format(_linkFormat,
                                        HttpContext.Current.Request.Url.Scheme,
                                        HttpContext.Current.Request.Url.Host, "{0}");

            PageType = pageType; // [ML] - Can be overriden if required
        }

        public string Title { get; set; }

        public string PageType { get; set; }

        public string Description { get; set; }

        public string PinterestDescription { get; set; }

        public string TwitterDescription { get; set; }

        public string ImageUrl { get; set; }

        public string ShareUrl { get; set; }

        public string RedirectUrl { get; set; }
    }
}
