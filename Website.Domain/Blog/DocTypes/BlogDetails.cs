using System;
using System.Web;
using Website.Domain.Service.DocTypes;
using Website.Domain.Shared.Enums;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Blog.DocTypes
{
    [ContentType(Name = "Blog Details", Description = "Blog details for the site", Controller = "Blog", Action = "Details", IconUrl = "icon-newspaper-alt")]
    public class BlogDetails : Page
    {
        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "Title *", Mandatory = true, Tab = Tabs.Content)]
        public string BlogTitle { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.RichtextEditor, Name = "Description *", Mandatory = true, Tab = Tabs.Content)]
        public IHtmlString BlogDescription { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Summary *", Mandatory = true, Tab = Tabs.Content)]
        public string BlogSummary { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.DatePicker, Name = "Publish Date *", Mandatory = true, Tab = Tabs.Content)]
        public DateTime BlogPublishDate { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TrueFalse, Name = "Disable Comments", Tab = Tabs.Content)]
        public bool DisableComments { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.Other, OtherTypeName = "Custom - Blog Categories", Name = "Category *", Mandatory = true, Tab = SiteTabs.Categorise)]
        public string BlogCategory { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.Tags, Name = "Tags *", Tab = SiteTabs.Categorise)]
        public string BlogTags { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.MediaPicker, Name = "Image", Tab = SiteTabs.Media)]
        public Image BlogImage { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Video", Tab = SiteTabs.Media)]
        public string BlogVideo { get; set; }
    }
}
