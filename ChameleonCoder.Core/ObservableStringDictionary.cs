using System.Runtime.InteropServices;

namespace System.Collections.Specialized
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.None), Guid("40fa63ee-5ced-4597-99cb-4baa7227d23c")]
    public class ObservableStringDictionary : StringDictionary, IObservableStringDictionary
    {
        public override string this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                string old = this[key];
                base[key] = value;
                OnItemReplaced(key, old, value);
            }
        }

        public override void Add(string key, string value)
        {
            base.Add(key, value);
            OnItemAdded(key, value);
        }

        public override void Clear()
        {
            base.Clear();
            OnCollectionCleared();
        }

        public override void Remove(string key)
        {
            string old = this[key];
            base.Remove(key);
            OnItemRemoved(key, old);
        }

        public ObservableStringDictionary Clone()
        {
            ObservableStringDictionary clone = new ObservableStringDictionary();
            foreach (DictionaryEntry entry in this)
            {
                clone.Add((string)entry.Key, (string)entry.Value);
            }
            return clone;
        }

        IObservableStringDictionary IObservableStringDictionary.Clone()
        {
            return Clone();
        }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [ComVisible(false)]
        protected void OnItemReplaced(string key, string oldValue, string newValue)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new DictionaryEntry(key, newValue), new DictionaryEntry(key, oldValue)));
        }

        [ComVisible(false)]
        protected void OnItemAdded(string key, string newValue)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new DictionaryEntry(key, newValue)));
        }

        [ComVisible(false)]
        protected void OnItemRemoved(string key, string oldValue)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new DictionaryEntry(key, oldValue)));
        }

        [ComVisible(false)]
        protected void OnCollectionCleared()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        [ComVisible(false)]
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion // "INotifyCollectionChanged"
    }
}
