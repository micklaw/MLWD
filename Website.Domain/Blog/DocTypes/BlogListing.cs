using System;
using Website.Domain.Service.DocTypes;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Blog.DocTypes
{
    [ContentType(Name = "Blog Listing", IsContainer = true, Description = "Blog listing for the site", Controller = "Blog", IconUrl = "icon-newspaper", AllowedChildNodeTypes = new[]
        {
            typeof(BlogDetails)
        })]
    public class BlogListing : Page
    {
        [ContentTypeProperty(ContentTypePropertyUI.Numeric, Name = "Page Item Count", Tab = Tabs.Content, DefaultValue = 5)]
        public int BlogPageCount { get; set; }
    }
}
