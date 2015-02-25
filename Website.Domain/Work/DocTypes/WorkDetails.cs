using System;
using System.Web;
using Website.Domain.Shared.Enums;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Work.DocTypes
{
    [ContentType(Name = "Work Details", Description = "Work details for the site", Controller = "Work", Action = "Details", IconUrl = "icon-newspaper-alt")]
    public class WorkDetails : Page
    {
        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "Title *", Mandatory = true, Tab = Tabs.Content)]
        public string WorkTitle { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.RichtextEditor, Name = "Description *", Mandatory = true, Tab = Tabs.Content)]
        public IHtmlString WorkDescription { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Summary *", Mandatory = true, Tab = Tabs.Content)]
        public string WorkSummary { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.DatePicker, Name = "Publish Date *", Mandatory = true, Tab = Tabs.Content)]
        public DateTime WorkPublishDate { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.Other, OtherTypeName = "Custom - Blog Categories", Name = "Category *", Mandatory = true, Tab = SiteTabs.Categorise)]
        public string WorkCategory { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.Tags, Name = "Tags *", Tab = SiteTabs.Categorise)]
        public string WorkTags { get; set; }


        [ContentTypeProperty(ContentTypePropertyUI.MediaPicker, Name = "Image", Tab = SiteTabs.Media)]
        public Image WorkImage { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.TextboxMultiple, Name = "Video", Tab = SiteTabs.Media)]
        public string WorkVideo { get; set; }
    }
}
