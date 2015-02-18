using System.Collections.Generic;
using Website.Domain.Blog.DocTypes;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Blog.ViewModels
{
    public class BlogListingViewModel
    {
        public BlogListing Listing { get; set; }

        public IList<Facet> Categories { get; set; }

        public IList<Facet> Tags { get; set; }
    }
}