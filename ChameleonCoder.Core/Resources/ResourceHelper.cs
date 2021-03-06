﻿using System;
using System.Xml;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.RichContent;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    /// <summary>
    /// a static class containing extension methods for IResource instances
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false), Obsolete]
    internal static class ResourceHelper
    {

        #region references

        /// <summary>
        /// adds a reference to the resource
        /// </summary>
        /// <param name="resource">the resource to add a reference on</param>
        /// <param name="name">the reference's name</param>
        /// <param name="target">the reference target</param>
        public static void AddReference(this IResource resource, string name, Guid target)
        {
            if (resource != null)
            {
                XmlNamespaceManager manager;
                var res = GetDataElement(resource, true, out manager);

                if (res != null)
                {
                    var element = (XmlElement)res.OwnerDocument.CreateElement(XmlDataFile.DocumentXPath.ResourceReferenceNode, XmlDataFile.NamespaceUri);
                    element.SetAttribute("name", XmlDataFile.NamespaceUri, name);
                    element.SetAttribute("id", XmlDataFile.NamespaceUri, Guid.NewGuid().ToString("b"));
                    element.SetAttribute("target", XmlDataFile.NamespaceUri, target.ToString("b"));
                    res.AppendChild(element);

                    var dict = new System.Collections.Specialized.ObservableStringDictionary();
                    foreach (XmlAttribute attr in element.Attributes)
                    {
                        dict.Add(attr.LocalName, attr.Value);
                    }
                    // todo: listen to dict changes
                    resource.AddReference(new Resources.ResourceReference(dict, resource.File));
                }
            }
        }

        /// <summary>
        /// deletes a reference from a resource
        /// </summary>
        /// <param name="resource">the resource to delete the reference from</param>
        /// <param name="id">the id of the reference to delete</param>
        public static void DeleteReference(this IResource resource, Guid id)
        {
            if (resource != null)
            {
                XmlNamespaceManager manager;
                var res = GetDataElement(resource, false, out manager);
                if (res != null)
                {
                    var element = (XmlElement)res.SelectSingleNode(XmlDataFile.DocumentXPath.ResourceReferenceSubpath + "[@id='" + id.ToString("b") + "']", manager);
                    if (element != null)
                        res.RemoveChild(element);
                }
            }
        }

        #endregion

        /// <summary>
        /// gets the resource-data element for the resource, optionally creating it if not found
        /// </summary>
        /// <param name="resource">the resource whose data should be found</param>
        /// <param name="create">true to create the lement if not found, false otherwise</param>
        /// <returns>the XmlElement containing the resource's data</returns>
        private static XmlElement GetDataElement(IResource resource, bool create, out XmlNamespaceManager manager)
        {
            var doc = ((XmlDataFile)resource.File).Document; // HACK!
            manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("cc", XmlDataFile.NamespaceUri);

            var data = (XmlElement)doc.SelectSingleNode(XmlDataFile.DocumentXPath.ResourceDataList + "[@cc:id='" + resource.Identifier.ToString("b") + "']", manager);
            if (data == null && create)
            {
                data = doc.CreateElement(XmlDataFile.DocumentXPath.ResourceDataNode, XmlDataFile.NamespaceUri); // create it
                data.SetAttribute("id", XmlDataFile.NamespaceUri, resource.Identifier.ToString("b")); // associate it with the resource
                doc.SelectSingleNode(XmlDataFile.DocumentXPath.DataRoot, manager).AppendChild(data); // and insert it into the document
            }

            return data;
        }
    }
}
