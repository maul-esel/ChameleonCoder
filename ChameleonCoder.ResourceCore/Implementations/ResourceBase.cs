using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// an abstract base class for resources
    /// </summary>
    public abstract partial class ResourceBase : IResource, INotifyPropertyChanged
    {
        static ResourceBase()
        {
            
        }

        #region IResource

        /// <summary>
        /// serves as base initializer for inherited classes and sets general properties
        /// </summary>
        /// <param name="node">the XmlNode that contains the resource</param>
        /// <param name="parent">the parent resource</param>
        public virtual void Init(XmlElement node, IResource parent)
        {
            Xml = node;
            Parent = parent;

            if (children == null)
                children = new ResourceCollection();

            GUID = new Guid(node.Attributes["guid"].Value);

            if (MetaData == null)
            {
                MetaData = new MetadataCollection();
                foreach (XmlNode meta in (from XmlNode meta in node.ChildNodes
                                          where meta.Name == "metadata"
                                          select meta))
                    MetaData.Add(new Metadata(meta));
            }
        }

        public XmlElement Xml { get; private set; }

        public abstract ImageSource Icon { get; }

        public Guid GUID { get; protected set; }

        [ResourceProperty(CommonResourceProperty.Name, ResourcePropertyGroup.General)]
        public virtual string Name
        {
            get
            {
                try
                {
                    return this.Xml.Attributes["name"].Value;
                }
                catch (NullReferenceException) { return string.Empty; }
            }
            set
            {
                this.Xml.Attributes["name"].Value = value;
                this.OnPropertyChanged("Name");
            }
        }

        [ResourceProperty(CommonResourceProperty.Description, ResourcePropertyGroup.General)]
        public virtual string Description
        {
            get
            {
                try
                {
                    return this.Xml.Attributes["description"].Value;
                }
                catch (NullReferenceException) { return null; }
            }
            set
            {
                this.Xml.Attributes["description"].Value = value;
                this.OnPropertyChanged("Description");
            }
        }

        public virtual string Notes
        {
            get
            {
                try { return this.Xml.Attributes["notes"].Value; }
                catch (NullReferenceException) { return string.Empty; }
            }
            set
            {
                this.Xml.Attributes["notes"].Value = value;
                this.OnPropertyChanged("Notes");
            }
        }

        public virtual MetadataCollection MetaData { get; protected set; }

        public virtual ImageSource SpecialVisualProperty { get { return null; } }

        public virtual IResource Parent { get; private set; }

        public virtual ResourceCollection children { get; private set; }

        #endregion 

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region PropertyAliases

        [ResourceProperty(CommonResourceProperty.Parent, ResourcePropertyGroup.General, IsReadOnly = true, IsReferenceName = true)]
        public string ParentName
        {
            get
            {
                if (Parent != null)
                    return Parent.Name;
                return string.Empty;
            }
        }

        [ResourceProperty(CommonResourceProperty.GUID, ResourcePropertyGroup.General, IsReadOnly = true)]
        public string GUIDName
        {
            get
            {
                return GUID.ToString("B");
            }
        }

        #endregion

        public static IResource Create(Type target, IResource parent, string name)
        {
            string parent_name = parent != null ? parent.Name : string.Empty;
            ResourceCreator creator = new ResourceCreator(target, parent_name, name);

            if (creator.ShowDialog() == true)
            {
                #region Xml

                XmlDocument doc = (parent == null) ? new XmlDocument() : parent.Xml.OwnerDocument;

                string alias = string.Empty;
                if (target == typeof(FileResource))
                    alias = "file";
                else if (target == typeof(CodeResource))
                    alias = "code";
                else if (target == typeof(LibraryResource))
                    alias = "library";
                else if (target == typeof(ProjectResource))
                    alias = "project";
                else if (target == typeof(LinkResource))
                    alias = "link";
                else if (target == typeof(TaskResource))
                    alias = "task";

                XmlElement node = doc.CreateElement(alias);

                foreach (KeyValuePair<string, string> pair in creator.GetXmlAttributes())
                {
                    XmlAttribute attr = doc.CreateAttribute(pair.Key);
                    attr.Value = pair.Value;
                    node.SetAttributeNode(attr);
                }
                (parent == null ? (XmlNode)doc : parent.Xml).AppendChild(node);
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

                return resource;
            }
            return null;
        }
    }
}
