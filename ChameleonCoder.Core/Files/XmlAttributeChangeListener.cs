using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Files
{
    partial class DataFile
    {
        [System.Runtime.InteropServices.ComVisible(false)]
        private sealed class XmlAttributeChangeListener
        {
            internal XmlAttributeChangeListener(IResource resource, XmlElement element)
            {
                resourceInstance = resource;
                resourceElement = element;
                resourceInstance.Attributes.CollectionChanged += OnCollectionChanged;
            }

            internal void Free()
            {
                resourceInstance = null;
                resourceElement = null;
                resourceInstance.Attributes.CollectionChanged -= OnCollectionChanged;
            }

            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        AddNewItems(args.NewItems);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        RemoveOldItems(args.OldItems);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        RemoveOldItems(args.OldItems);
                        AddNewItems(args.NewItems);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        resourceElement.Attributes.RemoveAll();
                        AddNewItems(args.NewItems); // todo: may be empty!
                        break;
                }
            }

            private void RemoveOldItems(System.Collections.IList items)
            {
                foreach (DictionaryEntry attr in items)
                {
                    resourceElement.RemoveAttribute(DocumentXPath.prefix + (string)attr.Key, DataFile.NamespaceUri);
                }
            }

            private void AddNewItems(System.Collections.IList items)
            {
                foreach (DictionaryEntry attr in items)
                {
                    resourceElement.SetAttribute((string)attr.Key, DataFile.NamespaceUri, (string)attr.Value);
                }
            }

            private IResource resourceInstance = null;
            private XmlElement resourceElement = null;
        }
    }
}
