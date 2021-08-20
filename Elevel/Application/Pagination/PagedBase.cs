using System;

namespace Elevel.Application.Pagination
{
    public abstract class PagedBase
    {
        /// <summary>
        /// Item count on page
        /// </summary>
        public int PageSize { get; set; } = 1000;

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        public int SkipCount() => (CurrentPage - 1) * PageSize;
    }
}