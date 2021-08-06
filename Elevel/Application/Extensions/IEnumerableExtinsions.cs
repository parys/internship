using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Extensions
{
    public static class IEnumerableExtinsions
    {
        public static async Task<IEnumerable<T>> WhereAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> func)
        {
            var elementList = new List<T>();
            foreach (var element in source)
            {
                if (await func(element))
                {
                    elementList.Add(element);
                }
            }
            
            return elementList;
        }
    }
}
