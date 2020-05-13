using System.Collections;
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

            // Compare special cases
            switch (lFirstValue)
            {
                case null:
                    return lSecondValue == null;
                case IDictionary lFirstDict when lSecondValue is IDictionary lSecondDict:
                    return lFirstDict.Keys
                        .Cast<object>()
                        .All(o => lSecondDict.Contains(o) && lFirstDict[o].Equals(lSecondDict[o]));
                case IEnumerable lFirstEnumerable when lSecondValue is IEnumerable lSecondEnumerable:
                    return lFirstEnumerable.Cast<object>().SequenceEqual(lSecondEnumerable.Cast<object>());
            }

            // If we don't have a special case, use the normal equals method to compare the two properties
            return lFirstValue.Equals(lSecondValue);
        }
    }
}