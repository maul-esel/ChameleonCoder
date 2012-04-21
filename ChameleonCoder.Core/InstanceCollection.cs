using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace ChameleonCoder
{
    /// <summary>
    /// an abstract generic base class for a keyed collection of instances
    /// </summary>
    /// <typeparam name="TKey">the type of the keys in the collection</typeparam>
    /// <typeparam name="TValue">the type of the values in the collection</typeparam>
    /// <remarks>This is used as base for
    /// the <see cref="ChameleonCoder.Resources.ResourceCollection"/>,
    /// the <see cref="ChameleonCoder.Resources.RichContent.RichContentCollection"/> and
    /// the <see cref="ChameleonCoder.Resources.ReferenceCollection"/> classes.</remarks>
    public abstract class InstanceCollection<TKey, TValue> : ObservableCollection<TValue>
    {
        ConcurrentDictionary<TKey, TValue> instances = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// adds a new instance to the collection. This must be called from within a derived class
        /// </summary>
        /// <param name="key">the key to use</param>
        /// <param name="value">the new instance</param>
        protected void Add(TKey key, TValue value)
        {
            if (!instances.ContainsKey(key))
            {
                instances.TryAdd(key, value);
                if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
                    ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() => base.Add(value)));
                else
                    base.Add(value);
            }
        }

        /// <summary>
        /// removes an instance from the collection
        /// </summary>
        /// <param name="key">the key of the instance to remove</param>
        public void Remove(TKey key)
        {
            TValue instance;
            instances.TryRemove(key, out instance);
            base.Remove(instance);
        }

        /// <summary>
        /// gets an instance from the collection
        /// </summary>
        /// <param name="key">the key of the instance to get</param>
        /// <returns>the instance, or null if it is not found</returns>
        public TValue GetInstance(TKey key)
        {
            TValue value;
            instances.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// adds, gets or sets an instance
        /// </summary>
        /// <param name="key">the key to use for the instance</param>
        /// <returns>the instance</returns>
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

        public TValue[] Values
        {
            get
            {
                TValue[] array = new TValue[instances.Values.Count];

                int index = 0;
                foreach (TValue value in instances.Values)
                {
                    array[index] = value;
                    index++;
                }

                return array;
            }
        }

        public TKey[] Keys
        {
            get
            {
                TKey[] array = new TKey[instances.Keys.Count];

                int index = 0;
                foreach (TKey key in instances.Keys)
                {
                    array[index] = key;
                    index++;
                }

                return array;
            }
        }
    }
}
