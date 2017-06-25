using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Azuria.Helpers.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static string ToString(this IEnumerable enumerable, char seperator)
        {
            return ToString(enumerable, seperator.ToString());
        }

        internal static string ToString(this IEnumerable enumerable, string seperator)
        {
            IEnumerable<object> lEnumerable = enumerable.Cast<object>();
            return lEnumerable.Aggregate(string.Empty, (o, o1) => string.Concat(o, seperator, o1))
                .Remove(0, seperator.Length);
        }
    }
}