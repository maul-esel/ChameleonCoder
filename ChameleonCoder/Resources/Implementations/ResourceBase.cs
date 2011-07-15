using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Implementations
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
            this.Xml = node;

            this.GUID = new Guid(node.Attributes["guid"].Value);

            this.MetaData = new MetadataCollection();
            foreach (XmlNode meta in (from XmlNode meta in node.ChildNodes
                                      where meta.Name == "metadata"
                                      select meta))
                this.MetaData.Add(new Metadata(meta));
        }

        #region IResource

        public XmlNode Xml { get; private set; }

        public abstract ImageSource Icon { get; }

        public Guid GUID { get; protected set; }

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
            protected set
            {
                this.Xml.Attributes["name"].Value = value;
                this.OnPropertyChanged("Name");
            }
        }

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
            protected set
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

        public virtual MetadataCollection MetaData { get; protected internal set; }

        public virtual ImageSource SpecialVisualProperty { get { return null; } }

        public virtual void Save()
        {
            foreach (Metadata data in this.MetaData)
            {
                data.Save(); // changes through the UI should be saved when they occur or through binding
            }
            this.Xml.OwnerDocument.Save(new Uri(this.Xml.BaseURI).LocalPath);
        }

        public abstract bool ValidateRichContent(RichContent.IContentMember member);

        public virtual void AddMetadata()
        {
            // ...

            this.Save();
        }

        public virtual void DeleteMetadata()
        {

        }

        public virtual void Move()
        {

        }

        public virtual void Delete()
        {

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

        #region IEnumerable<T>

        public virtual IEnumerator<PropertyDescription> GetEnumerator()
        {
            yield return new PropertyDescription("name", this.Name, "General");
            yield return new PropertyDescription("GUID", this.GUID.ToString("b"), "General");
            yield return new PropertyDescription("Description", this.Description, "General");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)this.GetEnumerator();
        }

        #endregion
    }
}
