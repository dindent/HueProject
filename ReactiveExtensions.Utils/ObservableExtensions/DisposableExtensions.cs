using System;
using System.Collections.Generic;

namespace ReactiveExtensions.Utils.ObservableExtensions
{
    public static class DisposableExtensions
    {
        public static IDisposable AsCompositeOf(this IDisposable disposable, ICollection<IDisposable> collection)
        {
            collection.Add(disposable);
            return disposable;
        }
    }
}