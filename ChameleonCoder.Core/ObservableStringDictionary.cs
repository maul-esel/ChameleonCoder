using System.Runtime.InteropServices;

namespace System.Collections.Specialized
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), Guid("40fa63ee-5ced-4597-99cb-4baa7227d23c")]
    public class ObservableStringDictionary : StringDictionary, INotifyCollectionChanged
    {
        public override string this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                base[key] = value;
                OnItemReplaced(key);
            }
        }

        public override void Add(string key, string value)
        {
            base.Add(key, value);
            OnItemAdded(key);
        }

        public override void Clear()
        {
            base.Clear();
            OnCollectionCleared();
        }

        public override void Remove(string key)
        {
            base.Remove(key);
            OnItemRemoved(key);
        }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [ComVisible(false)]
        protected void OnItemReplaced(string key)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, key));
        }

        [ComVisible(false)]
        protected void OnItemAdded(string key)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key));
        }

        [ComVisible(false)]
        protected void OnItemRemoved(string key)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key));
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
