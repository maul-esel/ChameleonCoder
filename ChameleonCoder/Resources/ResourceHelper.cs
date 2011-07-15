using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder
{
    internal static class ResourceHelper
    {
        public static void Save(this IResource resource)
        {
            IAllowChildren parentResource = resource as IAllowChildren;
            if (parentResource != null)
                foreach (IResource child in parentResource.children)
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
                IAllowChildren parentResource = resource as IAllowChildren;
                if (parentResource != null)
                    foreach (IResource child in parentResource.children)
                        child.Delete();

                resource.Xml.RemoveAll();
                resource.Xml.OwnerDocument.Save(new Uri(resource.Xml.BaseURI).LocalPath);
            }
        }

        public static void Move(this IResource resource, IAllowChildren newParent)
        {
            newParent.children.Add(resource);
            newParent.Xml.AppendChild(resource.Xml.Clone());
            resource.Xml.RemoveAll();

            // remove from old parent collection - how to find?
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
    }
}
