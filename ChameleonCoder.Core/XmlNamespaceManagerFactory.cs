using System.Collections.Generic;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// a class for managing XmlNamespaceManager instances
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)] // todo: make private subclass of DataFile
    internal static class XmlNamespaceManagerFactory
    {
        /// <summary>
        /// gets a manager for the given document. The manager contains a mapping of the namespace prefix "cc" to the URI in <see cref="Files.DataFile.NamespaceUri"/>.
        /// </summary>
        /// <param name="doc">the document instance to get the manager for</param>
        /// <returns>the XmlNamespacemanager instance</returns>
        internal static XmlNamespaceManager GetManager(XmlDocument doc)
        {
            if (loadedManagers.ContainsKey(doc))
                return loadedManagers[doc];

            var manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("cc", Files.DataFile.NamespaceUri);
            loadedManagers.Add(doc, manager);

            return manager;
        }

        /// <summary>
        /// clears the list of managers
        /// </summary>
        internal static void ClearManagers()
        {
            loadedManagers.Clear();
        }

        /// <summary>
        /// the private list holding the manager instances already created
        /// </summary>
        private static Dictionary<XmlDocument, XmlNamespaceManager> loadedManagers = new Dictionary<XmlDocument, XmlNamespaceManager>();
    }
}
