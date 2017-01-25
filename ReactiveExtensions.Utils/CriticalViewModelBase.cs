using ReactiveExtensions.Utils.Interfaces;
using Sogitec.IOS.Wpf.Utils.ViewModels;

namespace ReactiveExtensions.Utils
{
    public abstract class CriticalViewModelBase : ViewModelBase, ICriticalPropertyUpdate
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