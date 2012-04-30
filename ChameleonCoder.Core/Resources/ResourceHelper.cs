using System;
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
        /// <summary>
        /// parses the RichContent for a RichContentResource
        /// </summary>
        /// <param name="resource">the resource to parse</param>
        internal static void MakeRichContent(this IRichContentResource resource)
        {
            XmlNamespaceManager manager;
            var res = GetDataElement(resource, false, out manager);
            if (res == null)
                return;

            var content = (XmlElement)res.SelectSingleNode(DataFile.DocumentXPath.RichContentNode, manager);
            if (content == null)
                return;

            var contentMemberMan = resource.File.App.ContentMemberMan;
            foreach (XmlElement node in content.ChildNodes)
            {
                Guid type;
                IContentMember member = null;

                if (Guid.TryParse(node.GetAttribute("type", DataFile.NamespaceUri), out type))
                {
                    member = contentMemberMan.CreateInstanceOf(type, node, null);
                }
                else if (Guid.TryParse(node.GetAttribute("fallback", DataFile.NamespaceUri), out type))
                {
                    member = contentMemberMan.CreateInstanceOf(type, node, null);
                }

                if (member != null)
                {
                    resource.AddContentMember(member);
                    foreach (XmlElement child in node.ChildNodes)
                    {
                        AddRichContent(child, member, contentMemberMan);
                    }
                }
            }
        }

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
                    var element = (XmlElement)res.OwnerDocument.CreateElement(DataFile.DocumentXPath.ResourceReferenceNode, DataFile.NamespaceUri);
                    element.SetAttribute("name", DataFile.NamespaceUri, name);
                    element.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b"));
                    element.SetAttribute("target", DataFile.NamespaceUri, target.ToString("b"));
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
                    var element = (XmlElement)res.SelectSingleNode(DataFile.DocumentXPath.ResourceReferenceSubpath + "[@id='" + id.ToString("b") + "']", manager);
                    if (element != null)
                        res.RemoveChild(element);
                }
            }
        }

        #endregion

        /// <summary>
        /// parses the RichContent child members of a given RichConhtentMember instance
        /// </summary>
        /// <param name="node">the XmlElement representing the child member</param>
        /// <param name="parent">the parent member</param>
        private static void AddRichContent(XmlElement node, IContentMember parent, ContentMemberManager contentMemberMan)
        {
            Guid type;
            IContentMember member = null;

            if (Guid.TryParse(node.GetAttribute("type", DataFile.NamespaceUri), out type))
            {
                member = contentMemberMan.CreateInstanceOf(type, node, null);
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", DataFile.NamespaceUri), out type))
            {
                member = contentMemberMan.CreateInstanceOf(type, node, null);
            }

            if (member != null)
            {
                parent.Children.Add(member);
                foreach (XmlElement child in node.ChildNodes)
                {
                    AddRichContent(child, member, contentMemberMan);
                }
            }
        }

        /// <summary>
        /// gets the resource-data element for the resource, optionally creating it if not found
        /// </summary>
        /// <param name="resource">the resource whose data should be found</param>
        /// <param name="create">true to create the lement if not found, false otherwise</param>
        /// <returns>the XmlElement containing the resource's data</returns>
        private static XmlElement GetDataElement(IResource resource, bool create, out XmlNamespaceManager manager)
        {
            var doc = ((DataFile)resource.File).Document; // HACK!
            manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("cc", DataFile.NamespaceUri);

            var data = (XmlElement)doc.SelectSingleNode(DataFile.DocumentXPath.ResourceDataList + "[@cc:id='" + resource.Identifier.ToString("b") + "']", manager);
            if (data == null && create)
            {
                data = doc.CreateElement(DataFile.DocumentXPath.ResourceDataNode, DataFile.NamespaceUri); // create it
                data.SetAttribute("id", DataFile.NamespaceUri, resource.Identifier.ToString("b")); // associate it with the resource
                doc.SelectSingleNode(DataFile.DocumentXPath.DataRoot, manager).AppendChild(data); // and insert it into the document
            }

            return data;
        }

        #region path

        [Obsolete("Use ResourceManager.GetResourceFromIdPath()")]
        public static IResource GetResourceFromPath(string path, string separator)
        {
            var start = Res.Item_Home + separator + Res.Item_List;
            if (path.StartsWith(start, StringComparison.Ordinal))
                path = path.Remove(0, start.Length);

            var collection = ChameleonCoderApp.RunningObject.ResourceMan.Children;
            string[] segments = path.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            IResource result = null;
            int i = 0;
            foreach (string segment in segments)
            {
                i++;
                foreach (IResource res in collection)
                {
                    if (res.Name != segment)
                        continue;
                    if (segments.Length > i)
                        collection = res.Children;
                    else if (segments.Length == i)
                        result = res;
                    break;
                }
            }
            return result;
        }

        #endregion
    }
}
