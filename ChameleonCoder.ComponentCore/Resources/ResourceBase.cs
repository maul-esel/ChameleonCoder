﻿using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// an abstract base class for resources
    /// </summary>
    public abstract partial class ResourceBase : IResource
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// fired when an important property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// fires the PropertyChanged event
        /// </summary>
        /// <param name="name">the name of the paorperty that changed</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IResource

        /// <summary>
        /// serves as base initializer for inherited classes and sets general properties
        /// </summary>
        /// <param name="data">the XmlNode that contains the resource</param>
        /// <param name="parent">the parent resource</param>
        public virtual void Update(XmlElement data, IResource parent, DataFile file)
        {
            referenceCollection.CollectionChanged += (s, e) => OnPropertyChanged("References");
            childrenCollection.CollectionChanged += (s, e) => OnPropertyChanged("Children");

            Xml = data;
            File = file;
            Parent = parent;

            string guid = Xml.GetAttribute("id", DataFile.NamespaceUri);
            Guid id;
            if (!Guid.TryParse(guid, out id))
            {
                id = Guid.NewGuid();
                Xml.SetAttribute("id", DataFile.NamespaceUri, id.ToString("b"));
            }
            Identifier = id;
        }

        /// <summary>
        /// gets the XmlElement representing the resource
        /// </summary>
        /// <value>This value is the XmlElement given to the resource in the <see cref="Update"/> method.</value>
        public XmlElement Xml { get; private set; }

        /// <summary>
        /// when overriden in a derived class, gets the resource's icon
        /// </summary>
        public abstract ImageSource Icon { get; }

        /// <summary>
        /// gets the resource's unique identifier
        /// </summary>
        public Guid Identifier { get; protected set; }

        public DataFile File { get; protected set; }

        /// <summary>
        /// gets a list containing the resource's references
        /// </summary>
        public ReferenceCollection References
        {
            get { return referenceCollection; }
        }

        private ReferenceCollection referenceCollection = new ReferenceCollection();

        /// <summary>
        /// gets or sets the resource's name
        /// </summary>
        /// <value>The value is taken from the "name" attribute in the resource's XML.</value>
        [ResourceProperty(CommonResourceProperty.Name, ResourcePropertyGroup.General)]
        public virtual string Name
        {
            get
            {
                return Xml.GetAttribute("name", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("name", DataFile.NamespaceUri, value);
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// gets or sets the resource's description
        /// </summary>
        /// <value>The value is taken from the "description" attribute in the resource's XML.</value>
        [ResourceProperty(CommonResourceProperty.Description, ResourcePropertyGroup.General)]
        public virtual string Description
        {
            get
            {
                return Xml.GetAttribute("description", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("description", DataFile.NamespaceUri, value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// gets or sets the resource's notes
        /// </summary>
        /// <value>The value is taken from the "notes" attribute in the resource's XML.</value>
        public virtual string Notes
        {
            get
            {
                return Xml.GetAttribute("notes", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("notes", DataFile.NamespaceUri, value);
                OnPropertyChanged("Notes");
            }
        }

        /// <summary>
        /// When overriden in a derived class, gets an icon indicating an important property's status
        /// </summary>
        public virtual ImageSource SpecialVisualProperty { get { return null; } }

        /// <summary>
        /// gets the current instance's parent resource
        /// </summary>
        /// <value>This value is given to the resource in the <see cref="Update"/> method.</value>
        public IResource Parent { get; private set; }

        /// <summary>
        /// gets the current instance's children
        /// </summary>
        public ResourceCollection Children
        {
            get { return childrenCollection; }
        }

        private ResourceCollection childrenCollection = new ResourceCollection();

        #endregion        

        #region PropertyAliases

        /// <summary>
        /// gets the name of the resource's parent resource
        /// </summary>
        /// <value>The <see cref="Name"/> property of the <see cref="Parent"/> resource.</value>
        [ResourceProperty(CommonResourceProperty.Parent, ResourcePropertyGroup.General, IsReadOnly = true)]
        public string ParentName
        {
            get
            {
                if (Parent != null)
                    return Parent.Name;
                return string.Empty;
            }
        }

        /// <summary>
        /// gets the resource's identifier as string
        /// </summary>
        /// <value>The <see cref="Identifier"/>, converted to a string in the form of <c>{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></value>
        [ResourceProperty(CommonResourceProperty.Identifier, ResourcePropertyGroup.General, IsReadOnly = true)]
        public string IdentifierName
        {
            get
            {
                return Identifier.ToString("b");
            }
        }
        #endregion
    }
}
