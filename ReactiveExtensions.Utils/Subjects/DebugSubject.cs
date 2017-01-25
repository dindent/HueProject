using System;
using System.Diagnostics;
using System.Reactive;
using System.Reflection;
using log4net;

namespace ReactiveExtensions.Utils.Subjects
{
    public class DebugSubject<T> : IObservable<T>
    {
        private readonly Action<Action, NotificationKind> debugAction;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            log.InfoFormat("Action {0} took {1} ms", actionType, stopWatch.ElapsedMilliseconds);
        }
    }
}