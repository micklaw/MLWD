using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MLWD.Umbraco.Collections;

namespace MLWD.Umbraco.Utils
{
    public static class EnumberableUtils
    {

        /// <summary>
        /// Converts an IQueryable to a PagedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        /// <summary>
        /// Converts an IEnumerable to a PagedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalCount">The total count.</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize, int totalCount)
        {
            return new PagedList<T>(source, index, pageSize, totalCount);
        }

        /// <summary>
        /// Converts an IEnumerable to a PagedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source)
        {
            int total = source.Count();
            return new PagedList<T>(source, 0, total, total);
        }

        public static IList<SelectListItem> ToSelectListItemList<T>(this IEnumerable<T> items, Func<T, string> value, Func<T, string> text)
        {
            var list = new List<SelectListItem>();
            foreach (var item in items)
            {
                list.Add(new SelectListItem() { Text = text(item), Value = value(item) });
            }

            return list;
        }

        public static IList<SelectListItem> ToSelectListItemList(this IEnumerable<string> items)
        {
            return items.ToSelectListItemList(s => s, s => s);
        }

    }
 
}
