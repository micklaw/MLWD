using System;
using System.Collections.Generic;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Contents.DocTypes;
using Website.Domain.Home.Models.Archetypes;
using Website.Domain.Service.DocTypes;
using Website.Domain.Shared.Constants;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Home.DocTypes
{
    [ContentType(Description = "Homepage for the site", AllowAtRoot = true, Controller = "Home", IconUrl = "icon-home", AllowedChildNodeTypes = new []
        {
            typeof(Services),
            typeof(ContentPage),
            typeof(Contact),
            typeof(BlogListing)
        })]
    public class Homepage : Page
    {
        [Archetype]
        [ContentTypeProperty(ContentTypePropertyUI.Other, OtherTypeName = DataTypes.Testimonials, Tab = Tabs.Content)]
        public IList<Testimonial> Testimonials { get; set; }
    }
}
