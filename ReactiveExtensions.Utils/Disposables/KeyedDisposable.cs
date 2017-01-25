using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveExtensions.Utils.Disposables
{
    public class KeyedDisposable<TKey, TDisposable> : IDisposable, IDictionary<TKey, TDisposable>
        where TDisposable : IDisposable
    {
        private readonly Dictionary<TKey, TDisposable> _mInnerDictionary = new Dictionary<TKey, TDisposable>();
        private readonly object _mLockObject = new object();

        public void Dispose()
        {
            foreach (var keyValuePair in _mInnerDictionary)
            {
                keyValuePair.Value.Dispose();
            }
            _mInnerDictionary.Clear();
        }

        public void Add(TKey key, TDisposable value)
        {
            lock (_mLockObject)
            {
                _mInnerDictionary.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _mInnerDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _mInnerDictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            lock (_mLockObject)
            {
                return _mInnerDictionary.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TDisposable value)
        {
            return _mInnerDictionary.TryGetValue(key, out value);
        }

        public ICollection<TDisposable> Values
        {
            get
            {
                return _mInnerDictionary.Values;
            }
        }

        public TDisposable this[TKey key]
        {
            get
            {
                return _mInnerDictionary[key];
            }
            set
            {
                lock (_mLockObject)
                {
                    _mInnerDictionary[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TDisposable> item)
        {
            lock (_mLockObject)
            {
                _mInnerDictionary.Add(item.Key, item.Value);
            }
        }

        public void Clear()
        {
            lock (_mLockObject)
            {
                foreach (var item in _mInnerDictionary)
                {
                    item.Value.Dispose();
                }
                _mInnerDictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TDisposable> item)
        {
            return _mInnerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TDisposable>[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                Add(array[i]);
            }
        }

        public int Count
        {
            get { return _mInnerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TDisposable> item)
        {
            lock (_mLockObject)
            {
                return _mInnerDictionary.Remove(item.Key);
            }
        }

        public bool RemoveWithDispose(TKey key)
        {
            TDisposable value = default(TDisposable);
            if (TryGetValue(key, out value))
            {
                value.Dispose();
                return Remove(key);
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TDisposable>> GetEnumerator()
        {
            return _mInnerDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (_mInnerDictionary as System.Collections.IEnumerable).GetEnumerator();
        }
    }
}