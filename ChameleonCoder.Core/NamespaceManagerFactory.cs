using System.Collections.Generic;
using System.Xml;

namespace ChameleonCoder
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal static class NamespaceManagerFactory
    {
        internal static XmlNamespaceManager GetManager(XmlDocument doc)
        {
            if (loadedManagers.ContainsKey(doc))
                return loadedManagers[doc];

            var manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("cc", Files.DataFile.NamespaceUri);
            loadedManagers.Add(doc, manager);

            return manager;
        }

        internal static void ClearManagers()
        {
            loadedManagers.Clear();
        }

        private static Dictionary<XmlDocument, XmlNamespaceManager> loadedManagers = new Dictionary<XmlDocument, XmlNamespaceManager>();
    }
}
