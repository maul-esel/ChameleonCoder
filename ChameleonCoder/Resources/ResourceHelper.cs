using System;
using System.Xml;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    internal static class ResourceHelper
    {
        #region Save & Delete
        /// <summary>
        /// deletes a resource by removing all references for it and deleting its XML
        /// </summary>
        /// <param name="resource">the resource to delete</param>
        public static void Delete(this IResource resource)
        {
            var list = ResourceManager.GetList();
            foreach (IResource child in resource.children) // remove references to all child resources
            {
                if (ResourceManager.ActiveItem == child)
                    ResourceManager.ActiveItem = null; // maybe this needs some unloading process?
                list.Remove(child);
            }

            if (ResourceManager.ActiveItem == resource)
                ResourceManager.ActiveItem = null; // (see above)            

            resource.Xml.ParentNode.RemoveChild(resource.Xml);

            if (resource.Parent == null)
                ResourceManager.GetChildren().Remove(resource);
            else
                resource.Parent.children.Remove(resource);

            list.Remove(resource.GUID);

            resource.GetResourceFile().Save(); // save changes
        }
        #endregion

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
        /// <param name="moveGUID">a bool defining whether the copy should receive the original GUID or not.</param>
        public static void Copy(this IResource resource, IResource newParent, bool moveGUID)
        {
            var doc = (newParent == null ? resource.GetResourceFile() : newParent.GetResourceFile()).Document;

            var element = (XmlElement)resource.Xml.CloneNode(true); // get a clone for the copy
            if (element.OwnerDocument != doc) //if we switch the document:
                element = (XmlElement)doc.ImportNode(element, true); // import the XmlElement

            if (moveGUID) // if the copy should receive the original GUID:
            {
                resource.Xml.SetAttribute("guid", Guid.NewGuid().ToString("n")); // set the GUID-attribute of the old instance
                resource.Init(resource.Xml, resource.Parent); // re-init it to apply the changes
            }
            else // if the copy receives a new GUID:
                element.SetAttribute("guid", Guid.NewGuid().ToString("n")); // set the approbriate attribute

            App.AddResource(element, newParent); // let the App class create an instance, add it to the lists, init it, ...

            resource.GetResourceFile().Save(); // save the documents
            if (newParent != null)
                newParent.GetResourceFile().Save();
            System.Windows.MessageBox.Show((resource.Xml.ParentNode != null).ToString());
        }

        /// <summary>
        /// copies a resource to a new parent, giving it a new GUID
        /// </summary>
        /// <param name="resource">the resource to copy</param>
        /// <param name="newParent">the new parent resource or null to make it a top-level resource</param
        /// <remarks>this is an overload for the IResource.Copy(IResource, bool) method,
        /// using <code>false</code> for <code>moveGUID</code>.</remarks>
        public static void Copy(this IResource resource, IResource newParent)
        {
            resource.Copy(newParent, false);
        }

        #region metadata
        public static void AddMetadata(this IResource resource, Resources.Metadata meta)
        {
            System.Xml.XmlNode node = resource.Xml.SelectSingleNode("metadata");
            //if (node != null)
                //node.AppendChild(meta.node);
        }

        public static void DeleteMetadata(this IResource resource, Resources.Metadata meta)
        {
            //resource.Xml.RemoveChild(meta.Xml);
            resource.MetaData.Remove(meta);
        }
        #endregion

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
            return resource.GetPath("\\");
        }

        public static IResource GetResourceFromPath(string path, string separator)
        {
            if (path.StartsWith("CC" + separator))
                path.Remove(0, 3);
            Resources.ResourceCollection collection = Resources.Management.ResourceManager.GetChildren();
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
                        collection = res.children;
                    else if (segments.Length == i)
                        result = res;
                    break;
                }
            }
            return result;
        }

        public static IResource GetResourceFromPath(string path)
        {
            return GetResourceFromPath(path, "\\");
        }
        #endregion

        public static DataFile GetResourceFile(this IResource resource)
        {
            return App.OpenFile;
            // this method doesn't make a lot of sense now.
            // However, it may whenever multiple files are allowed to be opened simultaneously.
        }
    }
}
