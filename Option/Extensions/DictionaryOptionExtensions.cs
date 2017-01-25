using System.Collections.Generic;

namespace Option.Extensions
{
    public static class DictionaryOptionExtensions
    {
        public static Option<TValue> GetIfExists<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value.AsOption();
            }
            return Option.Empty<TValue>();
        }

        public static IDictionary<TKey, TValue> AddWhenHasValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Option<TValue> value)
        {
            value.WhenHasValue(v => dictionary.Add(key, v));
            return dictionary;
        }
    }
}