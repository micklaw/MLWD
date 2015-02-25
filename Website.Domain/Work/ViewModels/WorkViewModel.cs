using Website.Domain.Work.DocTypes;
using Yomego.CMS.Core.Collections;

namespace Website.Domain.Work.ViewModels
{
    public class WorkViewModel
    {
        public WorkListing Content { get; set; }

        public IPagedList<WorkDetails> Works { get; set; }
    }
}