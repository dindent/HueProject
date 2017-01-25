using ReactiveExtensions.Utils.Interfaces;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
// ReSharper disable once RedundantUsingDirective
using ReactiveExtensions.Utils.Interfaces;
using Utils;


namespace ReactiveExtensions.Utils.ObservableExtensions
{
    public static class WpfExtensions
    {
        public static IObservable<TValue> ObserveProperty<TViewModel, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TValue>> observedProperty, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged
        {
            var propertyName = observedProperty.GetPropertyName();
            var accessor = observedProperty.Compile();

            var sequence = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h,
                h => viewModel.PropertyChanged -= h)
                .Where(args => args.EventArgs.PropertyName == propertyName)
                .Select(_ => accessor(viewModel));

            if (startWithCurrentValue)
            {
                return sequence.StartWith(accessor(viewModel));
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveProperties<TViewModel, TResult1, TResult2, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TResult1>> observedProperty1, Expression<Func<TViewModel, TResult2>> observedProperty2, Func<TResult1, TResult2, TValue> resultSelector, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged
        {
            var sequence = viewModel.ObserveProperty(observedProperty1, startWithCurrentValue: true)
                .CombineLatest(viewModel.ObserveProperty(observedProperty2, startWithCurrentValue: true), resultSelector);
            if (!startWithCurrentValue)
            {
                sequence = sequence.Skip(1);
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveProperties<TViewModel, TResult1, TResult2, TResult3, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TResult1>> observedProperty1, Expression<Func<TViewModel, TResult2>> observedProperty2, Expression<Func<TViewModel, TResult3>> observedProperty3, Func<TResult1, TResult2, TResult3, TValue> resultSelector, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged
        {
            var sequence = viewModel.ObserveProperty(observedProperty1, startWithCurrentValue: true)
                .CombineLatest(viewModel.ObserveProperty(observedProperty2, startWithCurrentValue: true), viewModel.ObserveProperty(observedProperty3, startWithCurrentValue: true), resultSelector);
            if (!startWithCurrentValue)
            {
                sequence = sequence.Skip(1);
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveProperties<TViewModel, TResult1, TResult2, TResult3, TResult4, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TResult1>> observedProperty1, Expression<Func<TViewModel, TResult2>> observedProperty2, Expression<Func<TViewModel, TResult3>> observedProperty3, Expression<Func<TViewModel, TResult4>> observedProperty4, Func<TResult1, TResult2, TResult3, TResult4, TValue> resultSelector, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged
        {
            var sequence = viewModel.ObserveProperty(observedProperty1, startWithCurrentValue: true)
                .CombineLatest(viewModel.ObserveProperty(observedProperty2, startWithCurrentValue: true),
                viewModel.ObserveProperty(observedProperty3, startWithCurrentValue: true),
                viewModel.ObserveProperty(observedProperty4, startWithCurrentValue: true),
                resultSelector);
            if (!startWithCurrentValue)
            {
                sequence = sequence.Skip(1);
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveCriticalProperty<TViewModel, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TValue>> observedProperty, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged, ICriticalPropertyUpdate
        {
            var propertyName = observedProperty.GetPropertyName();
            var accessor = observedProperty.Compile();

            var sequence = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h,
                h => viewModel.PropertyChanged -= h)
                .Where(args => args.EventArgs.PropertyName == propertyName && !viewModel.IsUpdating)
                .Select(_ => accessor(viewModel));

            if (startWithCurrentValue)
            {
                return sequence.StartWith(accessor(viewModel));
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveCriticalProperties<TViewModel, TResult1, TResult2, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TResult1>> observedProperty1, Expression<Func<TViewModel, TResult2>> observedProperty2, Func<TResult1, TResult2, TValue> resultSelector, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged, ICriticalPropertyUpdate
        {
            var sequence = viewModel.ObserveCriticalProperty(observedProperty1, startWithCurrentValue: true)
                .CombineLatest(viewModel.ObserveCriticalProperty(observedProperty2, startWithCurrentValue: true), resultSelector);
            if (!startWithCurrentValue)
            {
                sequence = sequence.Skip(1);
            }
            return sequence;
        }

        public static IObservable<TValue> ObserveCriticalProperties<TViewModel, TResult1, TResult2, TResult3, TValue>(this TViewModel viewModel, Expression<Func<TViewModel, TResult1>> observedProperty1, Expression<Func<TViewModel, TResult2>> observedProperty2, Expression<Func<TViewModel, TResult3>> observedProperty3, Func<TResult1, TResult2, TResult3, TValue> resultSelector, bool startWithCurrentValue = false)
            where TViewModel : INotifyPropertyChanged, ICriticalPropertyUpdate
        {
            var sequence = viewModel.ObserveCriticalProperty(observedProperty1, startWithCurrentValue: true)
                .CombineLatest(viewModel.ObserveCriticalProperty(observedProperty2, startWithCurrentValue: true),
                viewModel.ObserveCriticalProperty(observedProperty3, startWithCurrentValue: true), resultSelector);
            if (!startWithCurrentValue)
            {
                sequence = sequence.Skip(1);
            }
            return sequence;
        }

        public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
        {
            private readonly string _propertyName;
            private readonly object _value;

            public PropertyValueChangedEventArgs(string propertyName, object value)
                : base(propertyName)
            {
                _propertyName = propertyName;
                _value = value;
            }

            public object Value
            {
                get { return _value; }
            }

            protected bool Equals(PropertyValueChangedEventArgs other)
            {
                return string.Equals(_propertyName, other._propertyName) && Equals(_value, other._value);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((PropertyValueChangedEventArgs)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_propertyName != null ? _propertyName.GetHashCode() : 0) * 397) ^ (_value != null ? _value.GetHashCode() : 0);
                }
            }
        }

        public static IObservable<EventPattern<PropertyValueChangedEventArgs>> ObservePropertiesChanges<TViewModel>(this TViewModel viewModel)
            where TViewModel : INotifyPropertyChanged
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h,
                h => viewModel.PropertyChanged -= h)
                .Select(pattern => new { pattern, property = viewModel.GetType().GetProperty(pattern.EventArgs.PropertyName) })
                .Where(obj => obj.property != null)
                .Select(
                    obj =>
                        new EventPattern<PropertyValueChangedEventArgs>(obj.pattern.Sender,
                            new PropertyValueChangedEventArgs(obj.pattern.EventArgs.PropertyName,
                                obj.property.GetValue(viewModel))));
        }

        public static IObservable<NotifyCollectionChangedEventArgs> ObserveCollection<TValue>(this ObservableCollection<TValue> collection)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => collection.CollectionChanged += h,
                h => collection.CollectionChanged -= h)
                .Select(args => args.EventArgs);
        }

        public static IObservable<TValue> WhenAdded<TValue>(this ObservableCollection<TValue> collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Add)
                .SelectMany(args => args.NewItems.ToObservable<TValue>());
        }

        public static IObservable<TValue> ToObservable<TValue>(this IList items)
        {
            return items.OfType<TValue>().ToObservable();
        }

        public static IObservable<TValue> WhenRemoved<TValue>(this ObservableCollection<TValue> collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Remove)
                .SelectMany(args => args.OldItems.ToObservable<TValue>());
        }

        public static IObservable<TValue> WhenMoved<TValue>(this ObservableCollection<TValue> collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Move)
                .SelectMany(args => args.NewItems.ToObservable<TValue>());
        }

        public static IObservable<NotifyCollectionChangedEventArgs> ObserveCollection(this INotifyCollectionChanged collection)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => collection.CollectionChanged += h,
                h => collection.CollectionChanged -= h)
                .Select(args => args.EventArgs);
        }

        public static IObservable<TValue> WhenAdded<TValue>(this INotifyCollectionChanged collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Add)
                .SelectMany(args => args.NewItems.ToObservable<TValue>());
        }

        public static IObservable<TValue> WhenRemoved<TValue>(this INotifyCollectionChanged collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Remove)
                .SelectMany(args => args.OldItems.ToObservable<TValue>());
        }

        public static IObservable<TValue> WhenMoved<TValue>(this INotifyCollectionChanged collection)
        {
            return collection.ObserveCollection().Where(args => args.Action == NotifyCollectionChangedAction.Move)
                .SelectMany(args => args.NewItems.ToObservable<TValue>());
        }

        public static IObservable<ListChangedEventArgs> ObserveCollection<TValue>(this BindingList<TValue> collection)
        {
            return Observable.FromEventPattern<ListChangedEventHandler, ListChangedEventArgs>(
                h => collection.ListChanged += h,
                h => collection.ListChanged -= h)
                .Select(args => args.EventArgs);
        }

        public static IObservable<TValue> WhenAdded<TValue>(this BindingList<TValue> collection)
        {
            return collection.ObserveCollection().Where(args => args.ListChangedType == ListChangedType.ItemAdded)
                .Select(args => collection[args.NewIndex]);
        }
    }
}