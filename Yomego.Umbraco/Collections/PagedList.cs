using System;
using System.Collections.Generic;
using System.Linq;

namespace Yomego.Umbraco.Collections
{
    [Serializable()]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageSize = pageSize;
            this.PageIndex = index + 1;
            this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source, int index, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageSize = pageSize;
            this.PageIndex = index + 1;
            this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source, int index, int pageSize, int totalCount)
        {
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.PageIndex = index + 1;
            this.AddRange(source);
        }

        public PagedList()
        {
            this.TotalCount = 0;
            this.PageSize = 10;
            this.PageIndex = 0;
        }

        public int TotalPages { get { return (int)Math.Ceiling((double)TotalCount / PageSize); } }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return ((PageIndex + 1) * PageSize) <= TotalCount;
            }
        }
    }
}
