using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;

namespace Website.Domain.Blog.DocTypes
{
    [UmbracoRoute("Blog", Action = "Details")]
    public class BlogDetails : Page
    {
        public BlogDetails(IPublishedContent content) : base(content) { }

        public string BlogTitle { get; set; }

        public IHtmlString BlogDescription { get; set; }

        public string BlogSummary { get; set; }

        public DateTime BlogPublishDate { get; set; }

        public bool DisableComments { get; set; }


        public string WorkTitle { get; set; }

        public string WorkClient { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image WorkClientImage { get; set; }

        public string WorkClientQuote { get; set; }

        public string WorkClientQuotePersonName { get; set; }

        public string WorkClientQuotePersonJob { get; set; }

        [TypeConverter(typeof(MultiImageConverter))]
        public IList<Image> WorkSlideshow { get; set; }


        public string BlogCategory { get; set; }

        public string BlogTags { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image BlogImage { get; set; }

        public string BlogVideo { get; set; }
    }
}
