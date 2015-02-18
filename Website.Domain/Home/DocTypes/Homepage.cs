using System;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Service.DocTypes;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Home.DocTypes
{
    [ContentType(Description = "Homepage for the site", AllowAtRoot = true, Controller = "Home", IconUrl = "icon-home", AllowedChildNodeTypes = new []
        {
            typeof(Services),
            typeof(BlogListing)
        })]
    public class Homepage : Page
    {
        
    }
}
