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
        public static void Save(this IResource resource)
        {
            if (resource != null)
            {
                foreach (IResource child in resource.children)
                    child.Save();

                IResolvable link = resource as IResolvable;
                if (link != null)
                    link.Resolve().Save();

                IRichContentResource richResource = resource as IRichContentResource;
                if (richResource != null)
                    foreach (Resources.RichContent.IContentMember content in richResource.RichContent)
                        content.Save();

                resource.Xml.OwnerDocument.Save(resource.GetResourceFile());
            }
        }

        public static void Delete(this IResource resource)
        {
            if (resource != null)
            {
                if (ResourceManager.ActiveItem == resource)
                    ResourceManager.ActiveItem = null;

                foreach (IResource child in resource.children)
                    child.Delete();

                string path = resource.GetResourceFile();
                if (resource.Parent != null)
                {
                    resource.Parent.Xml.RemoveChild(resource.Xml);
                    resource.Xml.OwnerDocument.Save(path);
                }
                else
                    System.IO.File.Delete(path);                    

                (resource.Parent == null ? ResourceManager.GetChildren() : resource.Parent.children).Remove(resource.GUID);
                ResourceManager.GetList().Remove(resource.GUID);
            }
        }
        #endregion

        public static void Move(this IResource resource, IResource newParent)
        {
            if (resource != null)
            {
                XmlDocument newDoc = newParent == null ? new XmlDocument() : newParent.Xml.OwnerDocument;

                XmlElement node = InformationProvider.CloneElement(resource.Xml, newDoc);

                (resource.Parent == null ? ResourceManager.GetChildren() : resource.Parent.children).Remove(resource.GUID);

                string path = resource.GetResourceFile();
                if (resource.Parent != null)
                {
                    resource.Parent.Xml.RemoveChild(resource.Xml);
                    resource.Xml.OwnerDocument.Save(path);
                }
                else
                    System.IO.File.Delete(path);

                (newParent == null ? ResourceManager.GetChildren() : newParent.children).Add(resource);
                (newParent == null ? (XmlNode)newDoc : newParent.Xml).AppendChild(node);

                resource.Init(node, newParent);
                if (newParent == null)
                {
                    path = InformationProvider.FindFreePath(App.DataDir, resource.Name + ".ccr", true);
                    newDoc.Save(path);
                    newDoc = new XmlDocument();
                    newDoc.Load(path);
                    resource.Init(newDoc.DocumentElement, newParent);
                }
                newDoc.Save(resource.GetResourceFile());
            }
        }

        public static void Copy(this IResource resource, IResource newParent)
        {
            if (resource != null)
            {
                XmlElement node = InformationProvider.CloneElement(resource.Xml, newParent.Xml.OwnerDocument);
                node.Attributes["guid"].Value = Guid.NewGuid().ToString("b");

                newParent.Xml.AppendChild(node);                

                IResource copy = Activator.CreateInstance(resource.GetType()) as IResource;
                copy.Init(node, newParent);
                ResourceManager.Add(copy, newParent);
            }
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

        public static string GetResourceFile(this IResource resource)
        {
            return new Uri(resource.Xml.BaseURI).LocalPath;
        }
    }
}
