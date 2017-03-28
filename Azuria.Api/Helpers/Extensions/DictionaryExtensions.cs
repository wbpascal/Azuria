using System;
using System.Collections.Generic;

namespace Azuria.Api.Helpers.Extensions
{
    internal static class DictionaryExtensions
    {
        #region Methods

        internal static void AddIf<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value,
            Func<TKey, TValue, bool> condition)
        {
            if (condition.Invoke(key, value)) source.Add(key, value);
        }

        internal static Dictionary<TKey, TValue> AddIfAndReturn<TKey, TValue>(this Dictionary<TKey, TValue> source,
            TKey key, TValue value, Func<TKey, TValue, bool> condition)
        {
            if (condition.Invoke(key, value)) source.Add(key, value);
            return source;
        }

        internal static Dictionary<TKey, TValue> AddIfAndReturn<TKey, TValue>(this Dictionary<TKey, TValue> source,
            TKey key, TValue value, Func<TKey, TValue, Dictionary<TKey, TValue>, bool> condition)
        {
            if (condition.Invoke(key, value, source)) source.Add(key, value);
            return source;
        }

        internal static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> source,
            IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (KeyValuePair<TKey, TValue> pair in keyValuePairs)
                source[pair.Key] = pair.Value;
        }

        internal static Dictionary<TValue, TKey> ReverseDictionary<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
            foreach (KeyValuePair<TKey, TValue> entry in source)
                if (!dictionary.ContainsKey(entry.Value))
                    dictionary.Add(entry.Value, entry.Key);
            return dictionary;
        }

        #endregion
    }
}