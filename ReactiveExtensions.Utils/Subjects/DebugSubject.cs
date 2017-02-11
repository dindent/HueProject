using System;
using System.Diagnostics;
using System.Reactive;

namespace ReactiveExtensions.Utils.Subjects
{
    public class DebugSubject<T> : IObservable<T>
    {
        private readonly Action<Action, NotificationKind> debugAction;
        private readonly IObservable<T> parent;

        public DebugSubject(IObservable<T> parent, Action<Action, NotificationKind> debugAction = null)
        {
            this.parent = parent;
            this.debugAction = debugAction ?? Debug;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return parent.Subscribe(
                v => debugAction(() => observer.OnNext(v), NotificationKind.OnNext),
                error => debugAction(() => observer.OnError(error), NotificationKind.OnError),
                () => debugAction(observer.OnCompleted, NotificationKind.OnCompleted));
        }

        private void Debug(Action action, NotificationKind actionType)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            action();
            stopWatch.Stop();
            System.Console.WriteLine("Action {0} took {1} ms", actionType, stopWatch.ElapsedMilliseconds);
        }
    }
}