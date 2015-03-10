using Yomego.CMS.Core.Collections;

namespace Yomego.CMS.Core.Umbraco.Model
{
    public class ApiResultsModel
    {
        public PagedList<Content> results { get; set; }

        public int resultCount { get; set; }

        public int totalCount { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public bool hasPrevious { get; set; }

        public bool hasNext { get; set; }

        public IPagedList<Content> ToPagedList()
        {
            if (results != null)
            {
                results.PageIndex = page;
                results.PageSize = pageSize;
                results.TotalCount = totalCount;
            }

            return results;
        }
    }
}