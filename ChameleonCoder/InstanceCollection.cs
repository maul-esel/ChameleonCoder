using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace ChameleonCoder
{
    public abstract class InstanceCollection<TKey, TValue> : ObservableCollection<TValue>
    {
        ConcurrentDictionary<TKey, TValue> instances = new ConcurrentDictionary<TKey, TValue>();

        protected void Add(TKey key, TValue value)
        {
            if (!instances.ContainsKey(key))
            {
                instances.TryAdd(key, value);
                if (!App.Current.Dispatcher.CheckAccess())
                    App.Current.Dispatcher.BeginInvoke(new Action(() => base.Add(value)));
                else
                    base.Add(value);
            }
        }

        public void Remove(TKey key)
        {
            TValue instance;
            instances.TryRemove(key, out instance);
            base.Remove(instance);
        }

        public TValue GetInstance(TKey key)
        {
            TValue value;
            instances.TryGetValue(key, out value);
            return value;
        }

        public TValue this[TKey key]
        {
            get
            {
                return GetInstance(key);
            }
            set
            {
                instances.AddOrUpdate(key, value, (k, v) => value);
            }
        }
    }
}
