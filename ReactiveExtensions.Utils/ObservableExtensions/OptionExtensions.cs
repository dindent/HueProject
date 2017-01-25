using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Option;
using Option.Extensions;

namespace ReactiveExtensions.Utils.ObservableExtensions
{
    public static class OptionExtensions
    {
        public static IObservable<T> WithValue<T>(this IObservable<Option<T>> sequence)
        {
            return sequence.Where(elt => elt.HasValue).Select(elt => elt.Value);
        }

        public static IObservable<T> ValueOrEmpty<T>(this Option<IObservable<T>> option)
        {
            return option.ValueOr(Observable.Empty<T>);
        }

        public static IDisposable ValueOrEmpty(this Option<IDisposable> option)
        {
            return option.ValueOr(Disposable.Empty);
        }

        public static IObservable<T> ValueOrThrow<T>(this Option<IObservable<T>> option, Func<Exception> exceptionFactory)
        {
            return option.ValueOr(() => Observable.Throw<T>(exceptionFactory()));
        }
    }
}