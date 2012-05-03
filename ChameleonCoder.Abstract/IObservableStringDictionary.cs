using System.Runtime.InteropServices;

namespace System.Collections.Specialized
{
    [ComVisible(true), Guid("CB50AE0C-C84A-4C68-99E7-719FCE81C7E2"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IObservableStringDictionary : INotifyCollectionChanged, IEnumerable
    {
        string this[string key] { get; set; }

        void Add(string key, string value);
        void Remove(string key);
        void Clear();

        ICollection Keys { get; }
        ICollection Values { get; }

        IObservableStringDictionary Clone();
    }
}
