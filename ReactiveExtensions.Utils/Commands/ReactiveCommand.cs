using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace ReactiveExtensions.Utils.Commands
{
    public class ReactiveCommand : IReactiveCommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Subject<object> executions = new Subject<object>();

        public ReactiveCommand(Func<object, bool> canExecute = null)
        {
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executions.OnNext(parameter);
        }

        public IObservable<object> ExecutionSequence
        {
            get { return executions; }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (this.canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
    }

    public class ReactiveCommand<T> : IReactiveCommand<T>
    {
        private readonly Func<T, bool> canExecute;
        private readonly Subject<T> executions = new Subject<T>();

        public ReactiveCommand(Func<T, bool> canExecute = null)
        {
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute((T)parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executions.OnNext((T)parameter);
        }

        public IObservable<T> TypedExecutionSequence
        {
            get { return executions; }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (this.canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        IObservable<object> IReactiveCommand.ExecutionSequence
        {
            get { return executions.Select(_ => (object)_); }
        }
    }
}