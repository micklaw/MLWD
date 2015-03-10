using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Umbraco.DocumentTypes
{
    [ContentType(Name = "Settings", Alias = "Settings", IconUrl = "icon-wrench", AllowAtRoot = true)]
    public class Settings : Content
    {
        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Tab = Tabs.Facebook, DefaultValue = null, Name = "Facebook App Id")]
        public string FacebookAppId { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Tab = Tabs.Facebook, DefaultValue = null, Name = "Facebook Secret")]
        public string FacebookSecret { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Tab = Tabs.Google, DefaultValue = null, Name = "Google Analytics")]
        public string GoogleAnalytics { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Alias = "umbracoNaviHide", Name = "Hide Page", Tab = Tabs.SEO, Description = "Hides the page from the sitemap etc")]
        public bool Hide { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Robots", Tab = Tabs.SEO, Description = "The robots.txt file to be used on the site")]
        public string Robots { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.Numeric, Name = "Api Paging", Tab = Tabs.Api, Mandatory = true, Description = "The paging for the api.")]
        public int ApiPageCount { get; set; }
    }
}
