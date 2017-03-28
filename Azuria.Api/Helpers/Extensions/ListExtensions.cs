using System;
using System.Collections.Generic;

namespace Azuria.Api.Helpers.Extensions
{
    internal static class ListExtensions
    {
        #region Methods

        internal static void AddIf<T>(this List<T> list, T item, Func<T, bool> condition)
        {
            if (condition.Invoke(item)) list.Add(item);
        }

        #endregion
    }
}