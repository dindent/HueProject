using ReactiveExtensions.Utils.Interfaces;

namespace ReactiveExtensions.Utils
{
    public abstract class CriticalViewModelBase : ICriticalPropertyUpdate
    {
        private bool isUpdating;

        public bool IsUpdating
        {
            get
            {
                return isUpdating;
            }
        }

        public void BeginUpdate()
        {
            isUpdating = true;
        }

        public void EndUpdate()
        {
            isUpdating = false;
        }
    }
}