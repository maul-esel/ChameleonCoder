using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace ChameleonCoder
{
    public abstract class InstanceCollection<TKey, TValue> : ObservableCollection<TValue>
    {
        ConcurrentDictionary<TKey, TValue> instances = new ConcurrentDictionary<TKey, TValue>();

        public void Add(TKey key, TValue value)
        {
            if (!instances.ContainsKey(key))
            {
                instances.TryAdd(key, value);
                if (!App.DispatcherObject.CheckAccess())
                    App.DispatcherObject.BeginInvoke(new Action(() => base.Add(value)));
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
            return instances[key];
        }

        public TValue this[TKey key]
        {
            get
            {
                return instances[key];
            }
            set
            {
                instances[key] = value;
            }
        }
    }
}
