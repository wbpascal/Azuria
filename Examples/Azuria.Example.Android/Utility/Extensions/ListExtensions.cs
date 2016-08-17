using System;
using System.Collections.Generic;

namespace Azuria.Example.Android.Utility.Extensions
{
    public static class ListExtensions
    {
        public static void AddIf<T1, T2>(this List<T1> list, IEnumerable<T2> toAdd, Func<List<T1>, T2, bool> condition)
            where T2 : T1
        {
            foreach (T2 objectToAdd in toAdd)
            {
                if (condition.Invoke(list, objectToAdd)) list.Add(objectToAdd);
            }
        }
    }
}