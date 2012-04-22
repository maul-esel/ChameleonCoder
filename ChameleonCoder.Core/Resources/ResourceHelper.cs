using System;
using System.Xml;
using ChameleonCoder.Files;
using ChameleonCoder.Resources.Interfaces;
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
        /// deletes a resource by removing all references for it and deleting its XML
        /// </summary>
        /// <param name="resource">the resource to delete</param>
        public static void Delete(this IResource resource)
        {
            var resMan = resource.File.App.ResourceMan;
            foreach (IResource child in resource.Children) // remove references to all child resources
            {
                if (resMan.ActiveResource == child)
                    resMan.Close(); // if a child is loaded: unload it
                resMan.Remove(child);
            }

            if (resMan.ActiveResource == resource)
                resMan.Close(); // unload the resource to delete

            resource.Xml.ParentNode.RemoveChild(resource.Xml);

            resMan.Remove(resource);

            resource.File.Save(); // save changes
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
            var file = newParent == null ? resource.File : newParent.File;
            var doc = ((DataFile)file).Document; // HACK!
            var manager = XmlNamespaceManagerFactory.GetManager(doc);

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
                resource.Update(resource.Xml, resource.Parent, file); // update it to apply the changes
            }
            else // if the copy receives a new Identifier:
                element.SetAttribute("id", DataFile.NamespaceUri, Guid.NewGuid().ToString("b")); // set the appropriate attribute

            ((DataFile)file).LoadResource(element, newParent); // let the DataFile class create an instance, add it to the lists, init it, ... // HACK!

            resource.File.Save(); // save the documents
            if (newParent != null)
                newParent.File.Save();
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

        /// <summary>
        /// parses the RichContent for a RichContentResource
        /// </summary>
        /// <param name="resource">the resource to parse</param>
        internal static void MakeRichContent(this IRichContentResource resource)
        {
            var res = GetDataElement(resource, false);
            if (res == null)
                return;

            var manager = XmlNamespaceManagerFactory.GetManager(res.OwnerDocument);
            var content = (XmlElement)res.SelectSingleNode("cc:richcontent", manager);
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
                    var manager = XmlNamespaceManagerFactory.GetManager(res.OwnerDocument);

                    foreach (XmlElement reference in res.SelectNodes("cc:references/cc:reference", manager))
                        resource.AddReference(new Resources.ResourceReference(reference, resource.File));
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

                    resource.AddReference(new Resources.ResourceReference(element, resource.File));
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
        internal static XmlElement GetDataElement(IResource resource, bool create)
        {
            var doc = ((DataFile)resource.File).Document; // HACK!
            var manager = XmlNamespaceManagerFactory.GetManager(doc);

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
