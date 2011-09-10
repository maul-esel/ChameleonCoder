using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;
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
        /// fires the ProeprtyChanged event
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
        public virtual void Initialize(XmlElement data, IResource parent)
        {
            Xml = data;
            Parent = parent;

            string guid = Xml.GetAttribute("id");
            Guid id;
            if (!Guid.TryParse(guid, out id))
            {
                id = Guid.NewGuid();
                Xml.SetAttribute("id", id.ToString("b"));
            }
            Identifier = id;
        }

        /// <summary>
        /// gets the XmlElement representing the resource
        /// </summary>
        public XmlElement Xml { get; private set; }

        /// <summary>
        /// when overriden in a derived class, gets the resource's icon
        /// </summary>
        public abstract ImageSource Icon { get; }

        /// <summary>
        /// gets the resource's unique identifier
        /// </summary>
        public Guid Identifier { get; protected set; }

        /// <summary>
        /// gets the resource's name
        /// </summary>
        [ResourceProperty(CommonResourceProperty.Name, ResourcePropertyGroup.General)]
        public virtual string Name
        {
            get
            {
                return Xml.GetAttribute("name");
            }
            set
            {
                Xml.SetAttribute("name", value);
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// gets the resource's description
        /// </summary>
        [ResourceProperty(CommonResourceProperty.Description, ResourcePropertyGroup.General)]
        public virtual string Description
        {
            get
            {
                return Xml.GetAttribute("description");
            }
            set
            {
                Xml.SetAttribute("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// gets the resource's notes
        /// </summary>
        public virtual string Notes
        {
            get
            {
                return Xml.GetAttribute("notes");
            }
            set
            {
                Xml.SetAttribute("notes", value);
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
