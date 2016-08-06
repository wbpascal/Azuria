using System;
using System.Collections.Generic;

namespace Azuria.Utilities.Extensions
{
    internal static class DictionaryExtensions
    {
        #region

        internal static Dictionary<T1, T2> AddIfAndReturn<T1, T2>(this Dictionary<T1, T2> source, T1 key, T2 value,
            Func<T1, T2, bool> condition)
        {
            if (condition.Invoke(key, value)) source.Add(key, value);
            return source;
        }

        internal static Dictionary<T1, T2> AddIfAndReturn<T1, T2>(this Dictionary<T1, T2> source, T1 key, T2 value,
            Func<T1, T2, Dictionary<T1, T2>, bool> condition)
        {
            if (condition.Invoke(key, value, source)) source.Add(key, value);
            return source;
        }

        #endregion
    }
}