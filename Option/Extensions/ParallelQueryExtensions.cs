using System;
using System.Linq;

namespace Option.Extensions
{
    public static class ParallelQueryExtensions
    {
        public static ParallelQuery<TResult> SelectWithValue<T, TResult>(this ParallelQuery<T> enumerable, Func<T, Option<TResult>> selector)
        {
            return enumerable.Select(selector).SelectWithValue();
        }

        public static ParallelQuery<T> SelectWithValue<T>(this ParallelQuery<Option<T>> enumerable)
        {
            return enumerable.Where(element => element.HasValue).Select(element => element.Value);
        }

        public static ParallelQuery<T> Do<T>(this ParallelQuery<T> enumerable, Action<T> action)
        {
            return enumerable.Select(value =>
            {
                action(value);
                return value;
            });
        }
    }
}