using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Home.DocTypes;
using Website.Domain.Home.ViewModels;
using Website.Domain.Shared.Search;
using Yomego.CMS.Core.Collections;
using Yomego.CMS.Core.Umbraco.Search;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Home.Controllers
{
    public class HomeController : BaseCMSController
    {
        public ActionResult Index()
        {
            var searchCrieria = SearchCriteria.WithPaging(0, 4).OrderByDescending(SearchOrder.PublishDate);

            var model = new HomeViewModel()
            {
                Content = Node as Homepage,
                Blogs = App.Services.Content.Get<BlogDetails>(searchCrieria) ?? new PagedList<BlogDetails>()
            };

            return View(model);
        }
    }
}
