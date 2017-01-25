using System.Reactive.Concurrency;

namespace ReactiveExtensions.Utils.Schedulers
{
    public class ReactiveSchedulers : IReactiveSchedulers
    {
        private readonly IScheduler currentThreadScheduler;
        private readonly IScheduler defaultScheduler;
        private readonly IDispatcherScheduler dispatcher;
        private readonly IScheduler immediateScheduler;
        private readonly INewThreadScheduler newThreadScheduler;
        private readonly ITaskScheduler taskScheduler;
        private readonly IThreadPoolScheduler threadPoolScheduler;

        public ReactiveSchedulers()
        {
            dispatcher = new ReactiveScheduler(DispatcherScheduler.Current);
            taskScheduler = new ReactiveScheduler(TaskPoolScheduler.Default);
            immediateScheduler = ImmediateScheduler.Instance;
            threadPoolScheduler = new ReactiveScheduler(ThreadPoolScheduler.Instance);
            newThreadScheduler = new ReactiveScheduler(NewThreadScheduler.Default);
            defaultScheduler = DefaultScheduler.Instance;
            currentThreadScheduler = CurrentThreadScheduler.Instance;
        }

        public IDispatcherScheduler Dispatcher
        {
            get { return dispatcher; }
        }

        public ITaskScheduler Task
        {
            get { return taskScheduler; }
        }

        public IScheduler Immediate
        {
            get { return immediateScheduler; }
        }

        public IThreadPoolScheduler ThreadPool
        {
            get { return threadPoolScheduler; }
        }

        public INewThreadScheduler NewThread
        {
            get { return newThreadScheduler; }
        }

        public IScheduler Default
        {
            get { return defaultScheduler; }
        }

        public IScheduler Current
        {
            get { return currentThreadScheduler; }
        }
    }
}