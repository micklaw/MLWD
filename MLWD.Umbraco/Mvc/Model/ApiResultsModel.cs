using MLWD.Umbraco.Collections;
using Umbraco.Core.Models.PublishedContent;

namespace MLWD.Umbraco.Mvc.Model
{
    public class ApiResultsModel
    {
        public PagedList<PublishedContentModel> results { get; set; }

        public int resultCount { get; set; }

        public int totalCount { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public bool hasPrevious { get; set; }

        public bool hasNext { get; set; }

        public IPagedList<PublishedContentModel> ToPagedList()
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