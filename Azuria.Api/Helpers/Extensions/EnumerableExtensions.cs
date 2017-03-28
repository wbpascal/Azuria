using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuria.Api.Helpers.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static string ToString(this IEnumerable enumerable, char seperator)
        {
            IEnumerable<object> lEnumerable = enumerable.Cast<object>();
            return lEnumerable.Aggregate(string.Empty, (o, o1) => string.Concat(o, seperator, o1));
        }
    }
}
