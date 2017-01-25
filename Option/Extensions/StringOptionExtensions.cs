namespace Option.Extensions
{
    public static class StringOptionExtensions
    {
        public static Option<string> GetContent(this string content)
        {
            return content.OnlyIf(!string.IsNullOrEmpty(content));
        }

        public static Option<string> GetContent(this Option<string> content)
        {
            return content.Select(c => c.GetContent());
        }

        public static string ValueOrEmpty(this Option<string> option)
        {
            return option.ValueOr(string.Empty);
        }
    }
}