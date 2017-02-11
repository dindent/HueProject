using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveExtensions.Utils.Subjects;

namespace ReactiveExtensions.Utils.ObservableExtensions
{
    public static class ObservableExtensions
    {
        public static IObservable<T> DelayEach<T>(this IObservable<T> observable, IObservable<TimeSpan> delays)
        {
            return observable.DelayEach(delays, Scheduler.Default);
        }

        public static IObservable<T> DelayEach<T>(this IObservable<T> observable, IObservable<TimeSpan> delays, IScheduler scheduler)
        {
            return delays
                .Select(delay => Observable.Interval(delay, scheduler))
                .Switch()
                .Zip(observable, (_, value) => value);
        }

        public static IObservable<T> WhenCompleted<T>(this IObservable<T> observable, Action onCompleted)
        {
            return observable.Do(_ => { }, onCompleted);
        }

        public static void Trig(this ISubject<Unit> subject)
        {
            subject.OnNext(Unit.Default);
        }

        public static IObservable<T> ConcatAfter<T>(this IObservable<T> observable,
            Func<IObservable<T>> nextObservableFactory)
        {
            var subject = new Subject<T>();
            var subject2 = new Subject<T>();
            var defer = Observable.Defer(nextObservableFactory);
            observable.Subscribe(subject);
            subject.Subscribe(_ => { }, ex => { }, () => defer.Subscribe(subject2));
            return subject.Concat(subject2);
        }

        public static IObservable<Unit> ToUnit<T>(this IObservable<T> observable)
        {
            return observable.Select(_ => Unit.Default);
        }

        public static IObservable<T> NotNull<T>(this IObservable<T> observable)
            where T : class
        {
            return observable.Where(value => value != null);
        }

        public static IObservable<TValue> SampleFirst<TValue, TSampler>(this IObservable<TValue> observable,
            IObservable<TSampler> sampler)
        {
            return sampler.Select(_ => observable.FirstAsync()).Switch();
        }

        public static IObservable<TValue> SampleFirst<TValue>(this IObservable<TValue> observable, TimeSpan sampleInterval)
        {
            return observable.SampleFirst(Observable.Timer(DateTimeOffset.Now, sampleInterval));
        }


        /// <summary>
        /// Gives a mean to easily debug observer calls (OnNExt, OnCompleted, OnError).
        /// This method is using a stopwatch and logs the time taken by each observers to perform their notification action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<T> ToDebug<T>(this IObservable<T> source)
        {
            return new DebugSubject<T>(source);
        }

        /// <summary>
        /// Gives a mean to easily debug observer calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="debugAction">First argument is the notification action to call on the observer (OnNext, OnError, OnCompleted). It should be called in the debugAction by the user.</param>
        /// <returns></returns>
        public static IObservable<T> ToDebug<T>(this IObservable<T> source, Action<Action, NotificationKind> debugAction)
        {
            return new DebugSubject<T>(source, debugAction);
        }
    }
}