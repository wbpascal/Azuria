using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Azuria.Helpers.Extensions
{
    internal static class EnumerableExtensions
    {
        #region Methods

        internal static string ToString(this IEnumerable enumerable, char seperator)
        {
            IEnumerable<object> lEnumerable = enumerable.Cast<object>();
            return lEnumerable.Aggregate(string.Empty, (o, o1) => string.Concat(o, seperator, o1));
        }

        #endregion
    }
}