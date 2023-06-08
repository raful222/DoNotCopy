using System.Collections.Generic;

namespace DoNotCopy.Core.UI.Search
{
    public interface IPagedList
    {
        string Search { get; set; }

        string SortBy { get; set; }

        string SortDir { get; set; }

        int PageCount { get; set; }

        int RecordTotal { get; set; }

        int Page { get; set; }

        int PageSize { get; set; }

        Dictionary<string, string> Query(
            string search = null,
            string sortBy = null,
            string sortDir = null,
            int? page = null,
            int? pageSize = null);
    }

    public interface IPagedList<TModel> : IPagedList
        where TModel : new()
    {
    }
}
