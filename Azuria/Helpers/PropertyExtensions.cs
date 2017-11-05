using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;

namespace Azuria.Helpers
{
    internal static class PropertyExtensions
    {
        public static bool ArePropertyValuesEqual<T>(this PropertyInfo property, T first, T second)
        {
            if (!typeof(T).GetTypeInfo().IsAssignableFrom(property.DeclaringType.GetTypeInfo())) return false;

            object lFirstValue = property.GetValue(first);
            object lSecondValue = property.GetValue(second);

            switch (lFirstValue)
            {
                case null:
                    return lSecondValue == null;
                case IEnumerable lFirstEnumerable when lSecondValue is IEnumerable lSecondEnumerable:
                    return lFirstEnumerable.Cast<object>().SequenceEqual(lSecondEnumerable.Cast<object>());
            }

            return lFirstValue.Equals(lSecondValue);
        }
    }
}