using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChameleonCoder
{
    public abstract class InstanceCollection<TKey, TValue> : ObservableCollection<TValue>
    {
        System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> instances = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>();

        object lock_add = new object();
        public void Add(TKey key, TValue value)
        {
            lock (lock_add)
            {
                if (!instances.ContainsKey(key))
                {
                    instances.TryAdd(key, value);
                    base.Add(value);
                }
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
            TValue instance;
            if (instances.TryGetValue(key, out instance))
                return instance;
            return default(TValue);
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.GetInstance(key);
            }
            set
            {
                instances.TryUpdate(key, value, value);
            }
        }
    }
}
