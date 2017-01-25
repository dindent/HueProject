using System;
using System.Collections.Generic;
using System.Linq;

namespace Option.Extensions
{
    public static class EnumerableOptionExtensions
    {
        public static IEnumerable<T> AppendWhenHasValue<T>(this IEnumerable<T> enumerable, Option<T> option)
        {
            return option.Select(v => enumerable.Concat(option.ToEnumerable())).ValueOr(enumerable);
        }

        public static IEnumerable<T> StartWithWhenHasValue<T>(this IEnumerable<T> enumerable, Option<T> option)
        {
            return option.Select(v => option.ToEnumerable().Concat(enumerable)).ValueOr(enumerable);
        }

        public static IEnumerable<T> SelectWithValue<T>(this IEnumerable<Option<T>> enumerable)
        {
            return enumerable.Where(element => element.HasValue).Select(element => element.Value);
        }

        public static IEnumerable<TResult> SelectWithValue<T, TResult>(this IEnumerable<T> enumerable, Func<T, Option<TResult>> selector)
        {
            return enumerable.Select(selector).SelectWithValue();
        }

        public static IEnumerable<T> ToEnumerable<T>(this Option<T> option)
        {
            if (option.HasValue)
            {
                yield return option.Value;
            }
            yield break;
        }

        public static Option<T> FirstAsOption<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            foreach (var value in enumerable.Where(predicate))
            {
                return value.AsOption();
            }
            return Option.Empty<T>();
        }

        public static Option<T> FirstAsOption<T>(this IEnumerable<T> enumerable)
        {
            foreach (var value in enumerable)
            {
                return value.AsOption();
            }
            return Option.Empty<T>();
        }

        public static Option<T> LastAsOption<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.Any())
            {
                return enumerable.Last().AsOption();
            }
            return Option.Empty<T>();
        }

        public static Option<T> LastAsOption<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var last = Option.Empty<T>();
            foreach (var item in enumerable.Where(predicate))
            {
                last = item.AsOption();
            }
            return last;
        }

        public static Option<T> SingleAsOption<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            using (var enumerator = enumerable.Where(predicate).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var value = enumerator.Current;
                    return value.OnlyIf(!enumerator.MoveNext());
                }
                return Option.Empty<T>();
            }
        }

        public static Option<T> SingleAsOption<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.SingleAsOption(_ => true);
        }
    }
}