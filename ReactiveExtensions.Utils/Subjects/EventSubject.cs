using System;
using Option;
using Option.Extensions;

namespace ReactiveExtensions.Utils.Subjects
{
    public class EventSubject<T> : IObserver<T>
    {
        public void OnNext(T value)
        {
            RaiseEvent(new Args {Value = value.AsOption()});
        }

        public void OnError(Exception error)
        {
            RaiseEvent(new Args {Exception = error.AsOption()});
        }

        public void OnCompleted()
        {
            RaiseEvent(new Args());
        }

        public event EventHandler<Args> Event;

        private void RaiseEvent(Args eventArgs)
        {
            if (Event != null)
            {
                Event(this, eventArgs);
            }
        }

        public struct Args
        {
            public Option<T> Value { get; set; }
            public Option<Exception> Exception { get; set; }

            public bool Completed
            {
                get { return !Value.HasValue && !Exception.HasValue; }
            }
        }
    }
}