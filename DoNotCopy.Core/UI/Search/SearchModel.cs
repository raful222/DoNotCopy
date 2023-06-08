using System;
using System.Collections.Generic;
using System.Text;

namespace DoNotCopy.Core.UI.Search
{
    public class SearchModel : ISearchModel
    {
        public string Search { get; set; }

        public string SortBy { get; set; }

        public virtual string SortDir { get; set; }

        public virtual int? Page { get; set; }

        public virtual int? PageSize { get; set; }
    }
}
