using System.Collections.Generic;

namespace ChameleonCoder
{
    /// <summary>
    /// generic abstract base collection class that can be used for Resources, RichContentMembers etc.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal abstract class ComponentCollection<TKey, TValue>
    {
        private Dictionary<TKey, TValue> components = new Dictionary<TKey, TValue>();

        protected void RegisterComponent(TKey key, TValue member)
        {
            components.Add(key, member);
        }

        protected TValue GetComponent(TKey key)
        {
            TValue value;
            if (components.TryGetValue(key, out value))
                return value;
            return default(TValue);
        }

        public IEnumerable<TValue> GetList()
        {
            return this.components.Values;
        }
    }
}
