using System.Collections.Generic;

namespace Elevel.Application.Pagination
{
    /// <summary>
    /// Strongly typed class for results and result set properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}