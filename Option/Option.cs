using Option.Interfaces;
using System;

namespace Option
{
    public struct Option<TValue> : IOption<TValue>
    {
        private readonly bool hasValue;
        private readonly TValue value;

        private Option(TValue value)
        {
            hasValue = true;
            this.value = value;
        }

        public bool HasValue
        {
            get { return hasValue; }
        }

        public TValue Value
        {
            get
            {
                if (HasValue)
                {
                    return value;
                }
                throw new InvalidOperationException("Option doesn't have a value.");
            }
        }

        internal static Option<TValue> FromValue(TValue value)
        {
            return new Option<TValue>(value);
        }
    }

    public static class Option
    {
        internal static Option<TValue> FromValue<TValue>(TValue value)
        {
            return Option<TValue>.FromValue(value);
        }

        public static Option<TValue> Empty<TValue>()
        {
            return default(Option<TValue>);
        }
    }
}