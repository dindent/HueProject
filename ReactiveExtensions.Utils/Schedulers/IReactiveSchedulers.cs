using System.Reactive.Concurrency;

namespace ReactiveExtensions.Utils.Schedulers
{
    public interface IReactiveSchedulers
    {
        IDispatcherScheduler Dispatcher { get; }

        ITaskScheduler Task { get; }

        IScheduler Immediate { get; }

        IThreadPoolScheduler ThreadPool { get; }

        INewThreadScheduler NewThread { get; }

        IScheduler Default { get; }

        IScheduler Current { get; }
    }
}