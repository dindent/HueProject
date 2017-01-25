namespace ReactiveExtensions.Utils.Interfaces
{
    public interface ICriticalPropertyUpdate
    {
        bool IsUpdating { get; }

        void BeginUpdate();

        void EndUpdate();
    }
}