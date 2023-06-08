using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNotCopy.Core.UI.Search
{
    public class PagedList<TModel> : List<TModel>, IPagedList<TModel>
         where TModel : new()
    {
        public static readonly int[] PageSizes = new int[] { 25, 50, 100 };


        public virtual string Search { get; set; }

        public virtual string SortBy { get; set; }

        public virtual string SortDir { get; set; }

        public virtual int Page { get; set; }

        public virtual int PageSize { get; set; }

        public virtual int PageCount { get; set; }

        public virtual int RecordTotal { get; set; }

        protected Dictionary<string, string> RouteData { get; set; }

        public string SortActiveClass(string sortBy, string sortDir = null, string cssClass = "active")
        {
            return SortBy == sortBy && SortDir == sortDir ? cssClass : "";
        }

        protected virtual Dictionary<string, string> Fill(
            Dictionary<string, string> routeData)
        {
            if (Search != null) { routeData[nameof(Search)] = Search; }
            if (SortBy != null) { routeData[nameof(SortBy)] = SortBy; }
            if (SortDir != null) { routeData[nameof(SortDir)] = SortDir; } else { if (routeData.ContainsKey(nameof(SortDir))) { routeData.Remove(nameof(SortDir)); } }
            if (Page > 1) { routeData[nameof(Page)] = Page.ToString(); }
            if (PageSize != PageSizes[0]) { routeData[nameof(PageSize)] = PageSize.ToString(); }

            return routeData;
        }

        public virtual Dictionary<string, string> Query(
            string search = null,
            string sortBy = null,
            string sortDir = null,
            int? page = null,
            int? pageSize = null)
        {
            var routeData = RouteData != null
                ? new Dictionary<string, string>(RouteData)
                : new Dictionary<string, string>();

            Fill(routeData);

            if (search != null) { routeData[nameof(Search)] = search; }
            if (sortBy != null) { routeData[nameof(SortBy)] = sortBy; }
            if (sortDir != null) { routeData[nameof(SortDir)] = sortDir; }
            if (page != null) { routeData[nameof(Page)] = page.ToString(); }
            if (pageSize != null) { routeData[nameof(PageSize)] = pageSize.ToString(); }

            return routeData;
        }

        public PagedList(IList<TModel> items,
            string search,
            string sortBy,
            string sortDir,
            int page,
            int pageSize,
            int pageCount,
            int recordTotal,
            Dictionary<string, string> routeValues)
        {
            Search = search;
            SortBy = sortBy;
            SortDir = sortDir;
            Page = page;
            PageSize = pageSize;
            PageCount = pageCount;
            RecordTotal = recordTotal;
            RouteData = routeValues;

            AddRange(items);
        }

        public static PagedList<TModel> ToPagedList(IQueryable<TModel> query, ISearchModel search, Dictionary<string, string> routeValues = null)
        {
            int page = search.Page > 0 ? search.Page.Value : 1;
            int pageSize = search.PageSize ?? PageSizes[0];

            int recordTotal = query.Count();
            int pageCount = (int)Math.Ceiling((decimal)recordTotal / pageSize);
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<TModel>(items, search.Search, search.SortBy, search.SortDir, page, pageSize, pageCount, recordTotal, routeValues);
        }

        public static async Task<PagedList<TModel>> ToPagedListAsync(IQueryable<TModel> query, ISearchModel search, Dictionary<string, string> routeValues = null)
        {
            int page = search.Page > 0 ? search.Page.Value : 1;
            int pageSize = search.PageSize ?? PageSizes[0];

            int recordTotal = await query.CountAsync();
            int pageCount = (int)Math.Ceiling((decimal)recordTotal / pageSize);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<TModel>(items, search.Search, search.SortBy, search.SortDir, page, pageSize, pageCount, recordTotal, routeValues);
        }
    }
}
