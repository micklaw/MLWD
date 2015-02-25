using System.Web.Mvc;
using Website.Domain.Blog.DocTypes;
using Website.Domain.Blog.ViewModels;
using Website.Domain.Shared.Search;
using Website.Domain.Work.DocTypes;
using Website.Domain.Work.ViewModels;
using Yomego.CMS.Core.Umbraco.Search;
using Yomego.CMS.Mvc.Controllers;

namespace Website.Domain.Work.Controllers
{
    public class WorkController : BaseCMSController
    {
        public ActionResult Index(int? p, string c, string t, string k)
        {
            var content = Node as WorkListing;

            var searchCrieria = SearchCriteria.WithWorkCategory(c)
                                              .AndWorkTag(t)
                                              .AndPaging(FixPage(p), content.WorkPageCount)
                                              .OrderByDescending(SearchOrder.PublishDate);

            var model = new WorkViewModel
            {
                Content = content,
                Works = App.Services.Content.Get<WorkDetails>(searchCrieria)
            };

            return View(model);
        }

        public ActionResult Details()
        {
            var model = new WorkDetailsViewModel()
            {
                Content = Node as WorkDetails
            };

            return View(model);
        }
    }
}
