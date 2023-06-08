using System;
using System.Collections.Generic;
using System.Text;

namespace DoNotCopy.Core.Helpers.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddIfNotNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : notnull
            where TValue : class
        {
            if (value != null) { dictionary.Add(key, value); }
        }

        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : notnull
            where TValue : class
        {
            if (value != null) { dictionary.Add(key, value); }
        }
    }
}
