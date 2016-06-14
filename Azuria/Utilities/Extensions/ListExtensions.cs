using System;
using System.Collections.Generic;

namespace Azuria.Utilities.Extensions
{
    internal static class ListExtensions
    {
        #region

        internal static void AddIf<T>(this List<T> list, T item, Func<T, bool> condition)
        {
            if (condition.Invoke(item)) list.Add(item);
        }

        #endregion
    }
}