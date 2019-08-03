using System;
using System.Collections.Generic;

namespace Azuria.Helpers.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static void AddIf<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TKey key, TValue value, Func<TKey, TValue, bool> condition)
        {
            if (condition.Invoke(key, value)) source.Add(key, value);
        }

        internal static void AddIfNotEmptyString<TKey>(this IDictionary<TKey, string> source, TKey key, string value)
        {
            source.AddIf(key, value, (key1, s) => !string.IsNullOrWhiteSpace(value));
        }

        internal static void AddOrUpdateRange<TKey, TValue>(
            this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (KeyValuePair<TKey, TValue> pair in keyValuePairs)
                source[pair.Key] = pair.Value;
        }
    }
}