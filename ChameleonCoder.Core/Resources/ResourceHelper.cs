using System;
using System.Collections.Generic;
using System.Xml;
using ChameleonCoder.Files;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.RichContent;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    /// <summary>
    /// a static class containing extension methods for IResource instances
    /// </summary>
    internal static class ResourceHelper
    {
        /// <summary>
        /// deletes a resource by removing all references for it and deleting its XML
        /// </summary>
        /// <param name="resource">the resource to delete</param>
        public static void Delete(this IResource resource)
        {
            foreach (IResource child in resource.Children) // remove references to all child resources
            {
                if (ResourceManager.ActiveItem == child)
                    ResourceManager.Close(); // if a child is loaded: unload it
                ResourceManager.Remove(child);
            }

            if (ResourceManager.ActiveItem == resource)
                ResourceManager.Close(); // unload the resource to delete            

            resource.Xml.ParentNode.RemoveChild(resource.Xml);

            ResourceManager.Remove(resource);

            resource.GetResourceFile().Save(); // save changes
        }

        #region Copy & Move
        /// <summary>
        /// moves a resource to a new parent resource
        /// </summary>
        /// <param name="resource">the resource to move</param>
        /// <param name="newParent">the new parent resource or null to make it a top-level resource</param>
        public static void Move(this IResource resource, IResource newParent)
        {
            if (resource.Parent == newParent)
                return;

            resource.Copy(newParent, true);
            resource.Delete();
        }

        /// <summary>
        /// copies a resource to a new parent
        /// </summary>
        /// <param name="resource">the resource to copy</param>
        /// <param name="newParent">the new parent resource or null to make it a top-level resource</param>
        /// <param name="moveGUID">a bool defining whether the copy should receive the original Identifier or not.</param>
        public static void Copy(this IResource resource, IResource newParent, bool moveGUID)
        {
            var file = newParent == null ? resource.GetResourceFile() : newParent.GetResourceFile();
            var doc = file.Document;
            var manager = NamespaceManagerFactory.GetManager(doc);

            var element = (XmlElement)resource.Xml.CloneNode(true); // get a clone for the copy
            if (element.OwnerDocument != doc) //if we switch the document:
                element = (XmlElement)doc.ImportNode(element, true); // import the XmlElement

            if (newParent == null) // if no parent:
                doc.SelectSingleNode("/cc:ChameleonCoder/cc:resources", manager).AppendChild(element); // add element to resource list
            else // if parent:
                newParent.Xml.AppendChild(element); // add element to parent's Children

            if (moveGUID) // if the copy should receive the original Identifier:
            {
                resource.Xml.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b")); // set the Identifier-attribute of the old instance
                resource.Update(resource.Xml, resource.Parent); // update it to apply the changes
            }
            else // if the copy receives a new Identifier:
                element.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b")); // set the appropriate attribute

            file.LoadResource(element, newParent); // let the DataFile class create an instance, add it to the lists, init it, ...

            resource.GetResourceFile().Save(); // save the documents
            if (newParent != null)
                newParent.GetResourceFile().Save();
        }

        /// <summary>
        /// copies a resource to a new parent, giving it a new Identifier
        /// </summary>
        /// <param name="resource">the resource to copy</param>
        /// <param name="newParent">the new parent resource or null to make it a top-level resource</param>
        /// <remarks>this is an overload for the IResource.Copy(IResource, bool) method,
        /// using <code>false</code> for <code>moveGUID</code>.</remarks>
        public static void Copy(this IResource resource, IResource newParent)
        {
            resource.Copy(newParent, false);
        }
        #endregion

        #region metadata
        /// <summary>
        /// sets the value of a resource's metadata with the specified key and creates it if necessary
        /// </summary>
        /// <param name="resource">the resource to receive the metadata</param>
        /// <param name="key">the metadata key</param>
        /// <param name="value">the value</param>
        public static void SetMetadata(this IResource resource, string key, string value)
        {
            var doc = resource.GetResourceFile().Document;
            var manager = NamespaceManagerFactory.GetManager(doc);

            // get the resource-data element for the resource
            XmlElement res = GetDataElement(resource, true);

            // get the metadata element for the given key
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null) // if it doesn't exist:
            {
                meta = doc.CreateElement("cc:metadata", DataFile.NamespaceUri); // create it
                meta.SetAttribute("key", DataFile.NamespaceUri, key); // give it the requested key
                res.AppendChild(meta); // and insert it
            }

            meta.SetAttribute("value", DataFile.NamespaceUri, value); // set the value
        }

        /// <summary>
        /// gets the value of a specified metadata element for the resource
        /// </summary>
        /// <param name="resource">the resurce containing the metadata</param>
        /// <param name="key">the metadata's key</param>
        /// <returns>the metadata's value if found, null otherwise</returns>
        public static string GetMetadata(this IResource resource, string key)
        {
            var doc = resource.GetResourceFile().Document;

            var manager = NamespaceManagerFactory.GetManager(doc);

            // get the resource's data element
            XmlElement res = GetDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return null; // there's no metadata --> return null

            // get the metadata element
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null) // if it doesn't exist:
                return null; // there's no such metadata --> return null

            return meta.InnerText; // return the requested value
        }

        /// <summary>
        /// gets a list of all metadata elements for the resource
        /// </summary>
        /// <param name="resource">the resource to analyze</param>
        /// <returns>a dictionary containing the metadata, which is empty if None is found</returns>
        public static Dictionary<string, string> GetMetadata(this IResource resource)
        {
            var doc = resource.GetResourceFile().Document;
            var manager = NamespaceManagerFactory.GetManager(doc);

            var dict = new Dictionary<string, string>();

            // get the resource's data element
            XmlElement res = GetDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return dict; // there's no metadata --> return empty dictionary

            var data = res.SelectNodes("cc:metadata/cc:metadata", manager); // get the list of metadata
            foreach (XmlElement meta in data)
            {
                var name = meta.GetAttribute("key", DataFile.NamespaceUri);
                if (!string.IsNullOrWhiteSpace(name) && !dict.ContainsKey(name))
                {
                    dict.Add(meta.GetAttribute("key", DataFile.NamespaceUri),
                        meta.GetAttribute("value", DataFile.NamespaceUri)); // add all metadata elements to the dictionary
                }
            }

            return dict; // return the dictionary
        }

        /// <summary>
        /// deletes a specified metadata
        /// </summary>
        /// <param name="resource">the resource to contain the metadata</param>
        /// <param name="key">the metadata's key</param>
        public static void DeleteMetadata(this IResource resource, string key)
        {
            var doc = resource.GetResourceFile().Document;
            var manager = NamespaceManagerFactory.GetManager(doc);

            // get the resource's data element
            XmlElement res = GetDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return; // there's no metadata --> return

            // get the metadata element
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']");
            if (meta == null) // if it doesn't exist:
                return; // there's no such metadata --> return

            meta.ParentNode.RemoveChild(meta); // remove the node
        }
        #endregion

        /// <summary>
        /// updates the "last-modified"-data of a resource to the current time
        /// </summary>
        /// <param name="resource">the resource to update</param>
        public static void UpdateLastModified(this IResource resource)
        {
            var res = GetDataElement(resource, true);
            var manager = NamespaceManagerFactory.GetManager(res.OwnerDocument);

            var lastmod = res.SelectSingleNode("cc:lastmodified", manager);

            if (lastmod == null)
            {
                lastmod = res.OwnerDocument.CreateElement("cc:lastmodified", DataFile.NamespaceUri);                
                res.AppendChild(lastmod);
            }

            lastmod.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// gets the "last-modified"-data of a resource
        /// </summary>
        /// <param name="resource">the resource to analyze</param>
        /// <returns>the last-modified DateTime, or <code>default(DateTime)</code> if it couldn't be found.</returns>
        public static DateTime GetLastModified(this IResource resource)
        {
            var res = GetDataElement(resource, false);
            if (res == null)
                return default(DateTime);

            var lastmod = res.SelectSingleNode("lastmodified");
            if (lastmod == null)
                return default(DateTime);

            return DateTime.Parse(lastmod.InnerText);
        }

        /// <summary>
        /// parses the RichContent for a RichContentResource
        /// </summary>
        /// <param name="resource">the resource to parse</param>
        internal static void MakeRichContent(this IRichContentResource resource)
        {
            var res = GetDataElement(resource, false);
            if (res == null)
                return;

            var manager = NamespaceManagerFactory.GetManager(res.OwnerDocument);
            var content = (XmlElement)res.SelectSingleNode("cc:richcontent", manager);
            if (content == null)
                return;

            foreach (XmlElement node in content.ChildNodes)
            {
                Guid type;
                IContentMember member = null;

                if (Guid.TryParse(node.GetAttribute("type", DataFile.NamespaceUri), out type))
                {
                    member = ContentMemberManager.CreateInstanceOf(type, node, null);
                }
                else if (Guid.TryParse(node.GetAttribute("fallback", DataFile.NamespaceUri), out type))
                {
                    member = ContentMemberManager.CreateInstanceOf(type, node, null);
                }

                if (member != null)
                {
                    resource.RichContent.Add(member);
                    foreach (XmlElement child in node.ChildNodes)
                    {
                        AddRichContent(child, member);
                    }
                }
            }
        }

        #region references

        /// <summary>
        /// loads all references
        /// </summary>
        /// <param name="resource">the resource to load the references on</param>
        internal static void LoadReferences(this IResource resource)
        {
            if (resource != null)
            {
                var res = GetDataElement(resource, false);

                if (res != null)
                {
                    var manager = NamespaceManagerFactory.GetManager(res.OwnerDocument);

                    foreach (XmlElement reference in res.SelectNodes("cc:references/cc:reference", manager))
                        resource.References.Add(new Resources.ResourceReference(reference));
                }
            }
        }

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
                var res = GetDataElement(resource, true);

                if (res != null)
                {
                    var element = (XmlElement)res.OwnerDocument.CreateElement("cc:reference", DataFile.NamespaceUri);
                    element.SetAttribute("name", DataFile.NamespaceUri, name);
                    element.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b"));
                    element.SetAttribute("target", DataFile.NamespaceUri, target.ToString("b"));
                    res.AppendChild(element);

                    resource.References.Add(new Resources.ResourceReference(element));
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
                var res = GetDataElement(resource, false);
                if (res != null)
                {
                    var element = (XmlElement)res.SelectSingleNode("cc:references/cc:reference[@id='" + id.ToString("b") + "']");
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
        private static void AddRichContent(XmlElement node, IContentMember parent)
        {
            Guid type;
            IContentMember member = null;

            if (Guid.TryParse(node.GetAttribute("type", DataFile.NamespaceUri), out type))
            {
                member = ContentMemberManager.CreateInstanceOf(type, node, null);
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", DataFile.NamespaceUri), out type))
            {
                member = ContentMemberManager.CreateInstanceOf(type, node, null);
            }

            if (member != null)
            {
                parent.Children.Add(member);
                foreach (XmlElement child in node.ChildNodes)
                {
                    AddRichContent(child, member);
                }
            }
        }

        /// <summary>
        /// gets the resource-data element for the resource, optionally creating it if not found
        /// </summary>
        /// <param name="resource">the resource whose data should be found</param>
        /// <param name="create">true to create the lement if not found, false otherwise</param>
        /// <returns>the XmlElement containing the resource's data</returns>
        internal static XmlElement GetDataElement(IResource resource, bool create)
        {
            var doc = resource.GetResourceFile().Document;
            var manager = NamespaceManagerFactory.GetManager(doc);

            var data = (XmlElement)doc.SelectSingleNode("/cc:ChameleonCoder/cc:data/cc:resourcedata[@cc:id='" + resource.Identifier.ToString("b") + "']", manager);
            if (data == null && create)
            {
                data = doc.CreateElement("cc:resourcedata", DataFile.NamespaceUri); // create it
                data.SetAttribute("id", DataFile.NamespaceUri, resource.Identifier.ToString("b")); // associate it with the resource
                doc.SelectSingleNode("/cc:ChameleonCoder/cc:data", manager).AppendChild(data); // and insert it into the document
            }

            return data;
        }

        #region path
        public static string GetPath(this IResource resource, string delimiter)
        {
            string path = string.Empty;

            while (resource != null)
            {
                path = delimiter + resource.Name + path;
                resource = resource.Parent;
            }
            return path;
        }

        public static string GetPath(this IResource resource)
        {
            return resource.GetPath(ChameleonCoderApp.resourcePathSeparator);
        }

        public static IResource GetResourceFromPath(string path, string separator)
        {
            var start = Res.Item_Home + separator + Res.Item_List;
            if (path.StartsWith(start, StringComparison.Ordinal))
                path = path.Remove(0, start.Length);

            var collection = ResourceManager.GetChildren();
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

        public static IResource GetResourceFromPath(string path)
        {
            return GetResourceFromPath(path, ChameleonCoderApp.resourcePathSeparator);
        }

        public static bool IsDescendantOf(this IResource resource, IResource ancestor)
        {
            return resource.GetPath().StartsWith(ancestor.GetPath(), StringComparison.Ordinal);
            // todo: find a better way to do this, as duplicate names are allowed
        }

        public static bool IsAncestorOf(this IResource resource, IResource descendant)
        {
            return descendant.IsDescendantOf(resource);
        }

        #endregion

        public static DataFile GetResourceFile(this IResource resource)
        {
            try
            {
                return ChameleonCoderApp.RunningObject.FileManager.GetResourceFile(resource.Xml.OwnerDocument);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("this resource's resource file cannot be detected: " + resource.Name, e);
            }            
        }
    }
}
