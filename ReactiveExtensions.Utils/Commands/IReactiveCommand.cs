using System;
using System.Windows.Input;

namespace ReactiveExtensions.Utils.Commands
{
    public interface IReactiveCommand<out T> : IReactiveCommand
    {
        IObservable<T> TypedExecutionSequence { get; }
    }

    public interface IReactiveCommand : ICommand
    {
        IObservable<object> ExecutionSequence { get; }
    }
}