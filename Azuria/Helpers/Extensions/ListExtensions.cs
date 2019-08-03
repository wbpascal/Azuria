using System;
using System.Collections.Generic;

namespace Azuria.Helpers.Extensions
{
    internal static class ListExtensions
    {
        internal static void AddIf<T>(this ICollection<T> list, T item, Func<T, bool> condition)
        {
            if (condition.Invoke(item)) list.Add(item);
        }
    }
}