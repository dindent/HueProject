using System;
using System.Reactive.Concurrency;

namespace ReactiveExtensions.Utils.Schedulers
{
    public class ReactiveScheduler : IThreadPoolScheduler, IDispatcherScheduler, INewThreadScheduler, ITaskScheduler
    {
        private readonly IScheduler _scheduler;

        public ReactiveScheduler(IScheduler scheduler)
        {
            this._scheduler = scheduler;
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, dueTime, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, dueTime, action);
        }

        public DateTimeOffset Now
        {
            get { return _scheduler.Now; }
        }
    }
}