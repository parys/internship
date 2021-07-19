using System;

namespace Elevel.Application.Pagination
{
    /// <summary>
    /// Base class used for paged data where type of data doesn't matter
    /// </summary>
    public class PagedResultBase : PagedBase
    {
        public int PageCount => (int)Math.Ceiling((double)RowCount / PageSize);

        public int RowCount { get; set; }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }
}