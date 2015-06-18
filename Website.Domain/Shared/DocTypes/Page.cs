using System.ComponentModel;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Website.Domain.Shared.Ditto.TypeConverters;
using Yomego.Umbraco.Mvc.Model.Media;

namespace Website.Domain.Shared.DocTypes
{
    public class Page : Yomego.Umbraco.Umbraco.Model.Content
    {
        public Page(IPublishedContent content) : base(content) { }

        public string MetaPageTitle { get; set; }

        public string MetaDescription { get; set; }


        public string OgTitle { get; set; }

        public string OgDescription { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image OgImage { get; set; }

        public bool OgUseThis { get; set; }


        [UmbracoProperty("UmbracoNaviHide")]
        public bool HidePage { get; set; }

        [UmbracoProperty("MvcViewName")]
        public string ViewPath { get; set; }
    }
}