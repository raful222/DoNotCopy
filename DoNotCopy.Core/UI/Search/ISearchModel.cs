
namespace DoNotCopy.Core.UI.Search
{
    public interface ISearchModel
    {
        string Search { get; set; }

        string SortBy { get; set; }

        string SortDir { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }
    }
}
