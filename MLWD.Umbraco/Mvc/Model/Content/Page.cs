using System.ComponentModel;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.Attributes;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace MLWD.Umbraco.Mvc.Model.Content
{
    public class Page : PublishedContentModel
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
