using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using Umbraco.Core.Models;
using System.Linq;
using Website.Domain.Shared.DocTypes;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Model.Media;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Blog.DocTypes
{
    [UmbracoRoute("Blog", Action = "Details")]
    public class BlogDetails : Page
    {
        public BlogDetails(IPublishedContent content) : base(content) { }

        public string BlogTitle { get; set; }

        [TypeConverter(typeof(HtmlStringConverter))]
        public HtmlString BlogDescription { get; set; }

        public string BlogSummary { get; set; }

        public DateTime BlogPublishDate { get; set; }

        public bool DisableComments { get; set; }


        public string WorkTitle { get; set; }

        public string WorkClient { get; set; }

        public string WorkClientSiteUrl { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image WorkClientImage { get; set; }

        public string WorkClientQuote { get; set; }

        public string WorkClientQuotePersonName { get; set; }

        public string WorkClientQuotePersonJob { get; set; }

        [TypeConverter(typeof(MultiImageConverter))]
        public IEnumerable<Image> WorkSlideshow { get; set; }


        public string BlogCategory { get; set; }

        public string BlogTags { get; set; }

        public IList<string> Tags
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BlogTags))
                {
                    return new List<string>();
                }

                return BlogTags.Split(',').Select(i => i.Trim()).Where( i => !string.IsNullOrWhiteSpace(i)).ToList();
            }
        }
        
    
        [TypeConverter(typeof(ImageConverter))]
        public Image BlogImage { get; set; }

        public string BlogVideo { get; set; }
    }
}
