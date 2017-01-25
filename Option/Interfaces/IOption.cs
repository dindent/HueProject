namespace Option.Interfaces
{
    public interface IOption<out TValue>
    {
        bool HasValue { get; }

        TValue Value { get; }
    }
}