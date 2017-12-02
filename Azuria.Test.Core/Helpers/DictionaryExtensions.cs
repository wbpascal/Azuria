using System.Collections.Generic;
using System.Linq;

namespace Azuria.Test.Core.Helpers
{
    public static class DictionaryExtensions
    {
        public static bool ContainsKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict, TKey key)
        {
            return dict.Any(pair => pair.Key.Equals(key));
        }

        public static IEnumerable<TValue> GetValue<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dict, TKey key)
        {
            return dict.Where(pair => pair.Key.Equals(key)).Select(pair => pair.Value);
        }
    }
}