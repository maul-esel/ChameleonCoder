using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChameleonCoder
{
    public abstract class InstanceCollection<TKey, TValue> : ObservableCollection<TValue>
    {
        Dictionary<TKey, TValue> instances = new Dictionary<TKey, TValue>();

        public void Add(TKey key, TValue value)
        {
            if (!instances.ContainsKey(key))
            {
                instances.Add(key, value);
                base.Add(value);
            }
        }

        public void Remove(TKey key)
        {
            TValue instance = this.GetInstance(key);
            instances.Remove(key);
            base.Remove(instance);
        }

        public TValue GetInstance(TKey key)
        {
            TValue instance;
            if (instances.TryGetValue(key, out instance))
                return instance;
            return default(TValue);
        }
    }
}
