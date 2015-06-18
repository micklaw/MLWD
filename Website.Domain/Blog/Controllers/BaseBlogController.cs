using System.Collections.Generic;
using System.Linq;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Blog.ViewModels;
using Website.Domain.Shared.Controllers;
using Yomego.Umbraco.Umbraco.Services.Search.Models;

namespace Website.Domain.Blog.Controllers
{
    public class BaseBlogController : AppController
    {
        private BlogListingViewModel GetBlogListingViewModel()
        {
            var model = new BlogListingViewModel
            {
                Listing = Node as BlogListing,
                Categories = App.Services.Content.FacetSearch(Criteria.WithFacetField("blogCategory").AndTypes(new[] {typeof (BlogDetails)}))
            };

            if (!string.IsNullOrWhiteSpace(Request.QueryString["c"]))
            {
                model.Keywords.Add("Category", Request.QueryString["c"]);
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["t"]))
            {
                model.Keywords.Add("Tags", Request.QueryString["t"]);
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["k"]))
            {
                model.Keywords.Add("Keywords", Request.QueryString["k"]);
            }

            if (model.Listing == null)
            {
                model.Listing = App.Services.Content.Get<BlogListing>().FirstOrDefault() ?? new List<BlogListing>().FirstOrDefault();
            }

            return model;
        }

        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            ViewData["Blog"] = GetBlogListingViewModel();

            base.OnActionExecuting(filterContext);
        }
    }
}
