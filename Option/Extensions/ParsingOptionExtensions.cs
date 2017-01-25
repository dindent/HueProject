using System;

namespace Option.Extensions
{
    public static class ParsingOptionExtensions
    {
        public static Option<TEnum> ParseAsEnum<TEnum>(this string value)
            where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse(value, true, out result))
            {
                return result.AsOption();
            }
            return Option.Empty<TEnum>();
        }

        public static Option<TResult> Parse<TResult>(this string value, Func<string, TResult> parser)
        {
            try
            {
                return parser(value).AsOption();
            }
            catch (Exception)
            {
                return Option.Empty<TResult>();
            }
        }
    }
}