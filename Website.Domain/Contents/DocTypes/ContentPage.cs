using System;
using System.Web;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Contents.DocTypes
{
    [ContentType(Name = "Content Page", Description = "A plain content page", Controller = "Content", Action = "Content", IconUrl = "icon-umb-content", AllowedChildNodeTypeOf = new[] { typeof(ContentPage) })]
    public class ContentPage : Page
    {
        [ContentTypeProperty(ContentTypePropertyUI.Textstring, Name = "Title *", Mandatory = true, Tab = Tabs.Content)]
        public string ContentTitle { get; set; }

        [ContentTypeProperty(ContentTypePropertyUI.RichtextEditor, Name = "Description *", Mandatory = true, Tab = Tabs.Content)]
        public IHtmlString ContentDescription { get; set; }
    }
}
