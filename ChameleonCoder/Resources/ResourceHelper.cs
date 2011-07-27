﻿using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder
{
    internal static class ResourceHelper
    {
        public static void Save(this IResource resource)
        {
            if (resource != null)
                foreach (IResource child in resource.children)
                    child.Save();

            IResolvable link = resource as IResolvable;
            if (link != null)
                link.Resolve().Save();

            IRichContentResource richResource = resource as IRichContentResource;
            if (richResource != null)
                //foreach(RichContent.IContentMember content in richResource.RichContent)
                richResource.ToString(); // todo!

            resource.Xml.OwnerDocument.Save(new Uri(resource.Xml.BaseURI).LocalPath);
        }

        public static void Delete(this IResource resource)
        {
            if (System.Windows.MessageBox.Show("all child resources will be deleted!", "deleting resource...", System.Windows.MessageBoxButton.OKCancel) != System.Windows.MessageBoxResult.Cancel)
            {
                if (resource != null)
                    foreach (IResource child in resource.children)
                        child.Delete();

                resource.Xml.RemoveAll();
                resource.Xml.OwnerDocument.Save(new Uri(resource.Xml.BaseURI).LocalPath);
            }
        }

        public static void Move(this IResource resource, IResource newParent)
        {
            System.Xml.XmlNode node;

            resource.Parent.children.Remove(resource.GUID);
            resource.Parent.Xml.RemoveChild(resource.Xml);

            newParent.children.Add(resource);
            newParent.Xml.AppendChild(node = resource.Xml.CloneNode(true));

            //resource.Xml = node;
            //resource.Parent = newParent;
        }

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

        public static string GetPath(this IResource resource, char delimiter)
        {
            string path = string.Empty;

            while (resource != null)
            {
                path = delimiter + resource.Name + path;
                resource = resource.Parent;
            }
            return path;
        }

        public static IResource GetResourceFromPath(string path)
        {
            if (path.StartsWith("CC\\"))
                path.Remove(0, 3);
            Resources.ResourceCollection collection = Resources.Management.ResourceManager.GetChildren();
            string[] segments = path.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            IResource result = null;
            int i = 1;
            foreach (string segment in segments)
            {
                foreach (IResource res in collection)
                {
                    System.Windows.MessageBox.Show(segment + "==" + res.Name);
                    if (res.Name == segment && segments.Length > i)
                    {
                        collection = res.children;
                        break;
                    }
                    else if (res.Name == segment && segments.Length == i)
                    {
                        result = res;
                        System.Windows.MessageBox.Show("result!");
                        break;
                    }
                    else
                        System.Windows.MessageBox.Show("no match");
                }
                i++;
            }
            return result;
        }

        public static string GetPath(this IResource resource)
        {
            return resource.GetPath('\\');
        }
    }
}
