using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuria.Utilities.Extensions
{
    internal static class ListExtensions
    {
        internal static void AddIf<T>(this List<T> list, T item, Func<T, bool> condition)
        {
            if (condition.Invoke(item)) list.Add(item);
        }
    }
}
