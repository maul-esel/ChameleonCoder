using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Interaction;
using System.Collections.Generic;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ResourceCore
{
    public sealed class ResourceProvider : ChameleonCoder.IComponentProvider
    {
        public void Init(Action<Type, ContentMemberTypeInfo> registerContentMember, Action<Type, ResourceTypeInfo> registerResourceType)
        {
            registerResourceType(typeof(LinkResource),
                new ResourceTypeInfo("link", "link", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/link.png")).GetAsFrozen() as ImageSource, Brushes.LightGreen.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(FileResource),
                new ResourceTypeInfo("file", "file", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/file.png")).GetAsFrozen() as ImageSource, Brushes.BurlyWood.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(CodeResource),
                new ResourceTypeInfo("code", "source code", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/code.png")).GetAsFrozen() as ImageSource, Brushes.LightSalmon.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(LibraryResource),
                new ResourceTypeInfo("library", "library", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/library.png")).GetAsFrozen() as ImageSource, Brushes.SandyBrown.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(ProjectResource),
                new ResourceTypeInfo("project", "project", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/project.png")).GetAsFrozen() as ImageSource, Brushes.Khaki.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(TaskResource),
                new ResourceTypeInfo("task", "task", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/task.png")).GetAsFrozen() as ImageSource, Brushes.LightCoral.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));

            registerResourceType(typeof(GroupResource),
                new ResourceTypeInfo("group", "group", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/group.png")).GetAsFrozen() as ImageSource, Brushes.DodgerBlue.GetAsFrozen() as Brush,
                    ResourceProvider.Create, ResourceProvider.Load, "maul.esel"));
        }

        public static void Load(IResource resource)
        {

        }

        public static void Create(Type target, IResource parent, Action<IResource, IResource> register)
        {
            string parent_name = parent != null ? parent.Name : string.Empty;
            ResourceCreator creator = new ResourceCreator(target, parent_name);

            if (creator.ShowDialog() == true)
            {
                #region Xml

                XmlDocument doc = (parent == null) ? new XmlDocument() : parent.Xml.OwnerDocument;
                XmlElement node = doc.CreateElement(InformationProvider.GetInfo(target).Alias);

                foreach (KeyValuePair<string, string> pair in creator.GetXmlAttributes())
                {
                    XmlAttribute attr = doc.CreateAttribute(pair.Key);
                    attr.Value = pair.Value;
                    node.SetAttributeNode(attr);
                }
                (parent == null ? doc : parent.Xml).AppendChild(node);
                #endregion

                IResource resource = Activator.CreateInstance(target) as IResource;
                resource.Init(node, parent);

                if (parent == null)
                {
                    string path = InformationProvider.FindFreePath(InformationProvider.DataDir, resource.Name + ".ccr", true);
                    doc.Save(path);
                    doc = new XmlDocument();
                    doc.Load(path);
                    resource.Init(doc.DocumentElement, parent);
                }

                register(resource, parent);
            }
        }
    }
}
