using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model.Interfaces;

namespace Yomego.CMS.Core.Umbraco.Model
{
    [ContentType]
    public class Partial : Content, IViewable
    {
        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Alias = "umbracoNaviHide", Name = "Hide Page", Description = "Hides the page from the sitemap etc")]
        public bool UmbracoNaviHide { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "View Name", Description = "An optional override that will attempt to use this view to render the content in Mvc.")]
        public string MvcViewName { get; set; }
    }
}
