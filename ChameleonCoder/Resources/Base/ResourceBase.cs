using System;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Base
{
    /// <summary>
    /// an abstract base class for resources
    /// </summary>
    public abstract partial class ResourceBase : IResource, INotifyPropertyChanged
    {
        /// <summary>
        /// serves as base constructor for inherited classes and sets general properties
        /// </summary>
        /// <param name="node">the XmlNode that contains the resource</param>
        public ResourceBase(XmlNode node)
        {
            this.XMLNode = node;

            this.GUID = new Guid(node.Attributes["guid"].Value);

            this.MetaData = new MetadataCollection();
            foreach (XmlNode meta in (from XmlNode meta in node.ChildNodes
                                      where meta.Name == "metadata"
                                      select meta))
                this.MetaData.Add(new Metadata(meta));

            this.children = new Collections.ResourceCollection();
        }

        protected XmlNode XMLNode;

        #region IResource

        public abstract ImageSource Icon { get; }

        /// <summary>
        /// the GUID that uniquely identifies the resource
        /// </summary>
        public Guid GUID { get; protected set; }

        public virtual ImageSource SpecialVisualProperty { get { return null; } }

        /// <summary>
        /// saves the information changed through the UI to the current instance and its XML representation
        /// </summary>
        public virtual void Save()
        {
            foreach (Metadata data in this.MetaData)
            {
                data.Save(); // changes through the UI should be saved when they occur or through binding
            }
            this.XMLNode.OwnerDocument.Save(new System.Uri(this.XMLNode.BaseURI).LocalPath);
        }

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

        /// <summary>
        /// the display name of the resource
        /// </summary>
        public virtual string Name
        {
            get
            {
                try
                {
                    return this.XMLNode.Attributes["name"].Value;
                }
                catch (NullReferenceException) { return string.Empty; }
            }
            protected set { this.XMLNode.Attributes["name"].Value = value; }
        }

        

        /// <summary>
        /// a short description of the resource
        /// </summary>
        public virtual string Description
        {
            get
            {
                try
                {
                    return this.XMLNode.Attributes["description"].Value;
                }
                catch (NullReferenceException) { return null; }
            }
            protected set { this.XMLNode.Attributes["description"].Value = value; }
        }

        /// <summary>
        /// the associated metadata as Metadata class instances
        /// </summary>
        public virtual MetadataCollection MetaData { get; protected internal set; }

        /// <summary>
        /// any notes related to the resource
        /// </summary>
        public virtual string Notes
        {
            get
            {
                try { return this.XMLNode.Attributes["notes"].Value; }
                catch (NullReferenceException) { return string.Empty; }
            }
            set { this.XMLNode.Attributes["notes"].Value = value; }
        }

        /// <summary>
        /// a ResourceCollection containing the children resources
        /// </summary>
        public virtual ResourceCollection children { get; protected set; }


        #region methods

        /// <summary>
        /// packages a resource into a zip file,
        /// including all attached or linked resources,
        /// all data files,
        /// any file a resource points to (FileResource, CodeResource, LibraryResource)
        /// </summary>
        public virtual void Package()
        {

        }

        

        /// <summary>
        /// adds a metadata element, given any changes through the UI
        /// it directly manipulates
        /// 1) the XmlDocument
        /// 2) the MetadataCollection
        /// </summary>
        internal virtual void AddMetadata()
        {
            // ...

            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void DeleteMetadata()
        {

        }

        /// <summary>
        /// attaches a copy of the resource to another resource
        /// to do so, a new GUID is created.
        /// a parameter or user input define where it goes:
        /// the original resource or the new copy
        /// </summary>
        internal virtual void AttachResource() // todo: parameter to define where the guid goes
        {

        }

        /// <summary>
        /// receives a resource that should be attached,
        /// adds it into the tree (-view), the children collection and the XmlDocument
        /// </summary>
        internal virtual void ReceiveAttach()
        {

        }

        /// <summary>
        /// links a resource to another resource
        /// </summary>
        internal virtual void LinkResource()
        {

        }

        /// <summary>
        /// receives a resource link
        /// </summary>
        internal virtual void ReceiveLink()
        {

        }

        /// <summary>
        /// moves a resource to another resource
        /// by first copying and then deleting it.
        /// When it is copied, the copy should receive the old GUID
        /// </summary>
        internal virtual void Move()
        {

        }

        /// <summary>
        /// deletes a resource by removing it from the tree (-view), any Collections and the XmlDocument
        /// </summary>
        internal virtual void Delete()
        {
            
        }

        public IResource Create() { System.Windows.MessageBox.Show("creation..."); return null; }
        public bool AddRichContentMember(ChameleonCoder.RichContent.IContentMember member) { return false; }

        /// <summary>
        /// overrides object.ToString()
        /// </summary>
        /// <returns>the name of the resource</returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
