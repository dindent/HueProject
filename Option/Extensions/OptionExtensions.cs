using Option.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Option.Extensions
{
    public static class OptionExtensions
    {
        public static Option<TValue> AsOption<TValue>(this Option<TValue> value)
        {
            return value;
        }

        public static Option<TValue> AsOption<TValue>(this TValue value)
        {
            if (value != null)
            {
                return Option.FromValue(value);
            }
            return Option.Empty<TValue>();
        }

        public static IOption<TValue> AsCovariant<TValue>(this Option<TValue> option)
        {
            return option;
        }

        public static Option<TValue> FromCovariant<TValue>(this IOption<TValue> option)
        {
            if (option != null && option.HasValue)
            {
                return option.Value.As<TValue>();
            }
            return Option.Empty<TValue>();
        }

        public static Option<TValue> As<TValue>(this object value)
        {
            if (value != null && typeof(TValue).GetTypeInfo().IsAssignableFrom(value.GetType()))
            {
                return Option.FromValue((TValue)value);
            }
            return Option.Empty<TValue>();
        }

        public static Option<TResult> Select<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> selector)
        {
            return option.Select(value => selector(value).AsOption());
        }

        /// <summary>
        /// This method performs the projection being safe on the selector execution, i.e. any exception is caught.
        /// If any exception occur, an empty option is returned by the operator.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="option"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Option<TResult> SafeSelect<TValue, TResult>(this Option<TValue> option,
            Func<TValue, TResult> selector)
        {
            return option.Select(value =>
            {
                try
                {
                    return selector(value).AsOption();
                }
                catch
                {
                    return Option.Empty<TResult>();
                }
            });
        }

        public static Option<TResult> Cast<TResult>(this IOption<TResult> option)
        {
            return option.FromCovariant().Select(value => value);
        }

        public static Option<TResult> Cast<TResult>(this IOption<object> option)
        {
            return option.FromCovariant().Select(value => value.As<TResult>());
        }

        public static Option<TResult> Select<TValue, TResult>(this Option<TValue> option, Func<TValue, Option<TResult>> selector)
        {
            return option.HasValue ? selector(option.Value) : Option.Empty<TResult>();
        }

        public static Option<TResult> OrElse<TResult>(this Option<TResult> option, Option<TResult> defaultValue)
        {
            return option.HasValue ? option : defaultValue;
        }

        public static Option<TResult> OrElse<TResult>(this Option<TResult> option, Func<TResult> valueFactory)
        {
            return option.OrElse(() => valueFactory().AsOption());
        }

        public static Option<TResult> OrElse<TResult>(this Option<TResult> option, Func<Option<TResult>> optionFactory)
        {
            return option.HasValue ? option : optionFactory();
        }

        public static TResult ValueOr<TResult>(this Option<TResult> option, TResult defaultValue)
        {
            return option.HasValue ? option.Value : defaultValue;
        }

        public static IEnumerable<TResult> ValueOrEmpty<TResult>(this Option<IEnumerable<TResult>> option)
        {
            return option.ValueOr(Enumerable.Empty<TResult>());
        }

        public static TResult ValueOrThrow<TResult>(this Option<TResult> option, Func<Exception> exceptionFactory)
        {
            if (option.HasValue)
            {
                return option.Value;
            }
            throw exceptionFactory();
        }

        public static TResult ValueOrThrow<TResult>(this Option<TResult> option, Exception exception)
        {
            if (option.HasValue)
            {
                return option.Value;
            }
            throw exception;
        }

        public static TResult ValueOr<TResult>(this Option<TResult> option, Func<TResult> valueFactory)
        {
            return option.HasValue ? option.Value : valueFactory();
        }

        public static TResult ValueOrNull<TResult>(this Option<TResult> option)
            where TResult : class
        {
            return option.HasValue ? option.Value : null;
        }

        public static Option<TValue> Where<TValue>(this Option<TValue> option, Predicate<TValue> condition)
        {
            return option.HasValue && condition(option.Value) ? option : Option.Empty<TValue>();
        }

        public static Option<TValue> OnlyIf<TValue>(this TValue value, Predicate<TValue> condition)
        {
            if (condition(value))
            {
                return value.AsOption();
            }
            return Option.Empty<TValue>();
        }

        public static Option<TValue> OnlyIf<TValue>(this TValue value, Func<bool> condition)
        {
            return value.OnlyIf(_ => condition());
        }

        public static Option<TValue> OnlyIf<TValue>(this TValue value, bool condition)
        {
            return value.OnlyIf(_ => condition);
        }

        public static Option<TValue> WhenHasValue<TValue>(this Option<TValue> option, Action<TValue> action)
        {
            if (option.HasValue)
            {
                action(option.Value);
            }
            return option;
        }

        /// <summary>
        /// Executes an action when the option has a value.
        /// If the action raises an exception, an empty option is returned instead of the original valued option.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="option"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Option<TValue> WhenHasValueSafe<TValue>(this Option<TValue> option, Action<TValue> action)
        {
            if (option.HasValue)
            {
                try
                {
                    action(option.Value);
                }
                catch (Exception)
                {
                    return Option.Empty<TValue>();
                }
            }
            return option;
        }

        public static Option<TValue> WhenEmpty<TValue>(this Option<TValue> option, Action action)
        {
            if (!option.HasValue)
            {
                action();
            }
            return option;
        }

        public static Option<TValue> ThrowWhenEmpty<TValue>(this Option<TValue> option, Exception ex)
        {
            return option.ThrowWhenEmpty(() => ex);
        }

        public static Option<TValue> ThrowWhenEmpty<TValue>(this Option<TValue> option, Func<Exception> exceptionFactory)
        {
            if (!option.HasValue)
            {
                throw exceptionFactory();
            }
            return option;
        }

        public static Option<TValue> Do<TValue>(this Option<TValue> option, Action<TValue> whenHasValue, Action whenEmpty)
        {
            return option
                .WhenHasValue(whenHasValue)
                .WhenEmpty(whenEmpty);
        }

        public static bool OptionEquals<TValue>(this Option<TValue> option, Option<TValue> other)
        {
            if (option.HasValue == false)
            {
                return other.HasValue == false;
            }
            return option.HasValue == other.HasValue && option.Value.Equals(other.Value);
        }

        public static bool ValueEquals<TValue>(this Option<TValue> option, TValue other)
        {
            return option.Select(v => v.Equals(other)).ValueOr(false);
        }

        public static bool IsEmpty<TValue>(this Option<TValue> option)
        {
            return !option.HasValue;
        }
    }
}