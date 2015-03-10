using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model.Interfaces;

namespace Yomego.CMS.Core.Umbraco.Model
{
    [ContentType()]
    public class Page : Content, IViewable
    {
        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "Meta Page Title", Tab = "SEO")]
        public string MetaPageTitle { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Meta Description", Tab = "SEO")]
        public string MetaDescription { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Meta Keywords", Tab = "SEO")]
        public string MetaKeywords { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "Og Title", Tab = "Og")]
        public string OgTitle { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Og Description", Tab = "Og")]
        public string OgDescription { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.MediaPicker, Name = "Og Image", Tab = "Og")]
        public Image OgImage { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Name = "Og Use This", Tab = "Og")]
        public bool OgUseThis { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Description = "Is this a mandatory footer page", Name = "Footer page *")]
        public string IsMandatory { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Alias = "umbracoNaviHide", Name = "Hide Page", Description = "Hides the page from the sitemap etc")]
        public bool UmbracoNaviHide { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "View Name", Description = "An optional override that will attempt to use this view to render the content in Mvc.")]
        public string MvcViewName { get; set; }
    }
}
