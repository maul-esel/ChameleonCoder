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

        public event PropertyChangedEventHandler PropertyChanged;

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
        /// <param name="node">the XmlNode that contains the resource</param>
        /// <param name="parent">the parent resource</param>
        public virtual void Init(XmlElement data, IResource parent)
        {
            Xml = data;
            Parent = parent;

            if (children == null)
                children = new ResourceCollection();

            string guid = Xml.GetAttribute("guid");
            Guid id;
            if (!Guid.TryParse(guid, out id))
            {
                id = Guid.NewGuid();
                Xml.SetAttribute("guid", id.ToString());
            }
            GUID = id;
        }

        public XmlElement Xml { get; private set; }

        public abstract ImageSource Icon { get; }

        public Guid GUID { get; protected set; }

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

        public virtual ImageSource SpecialVisualProperty { get { return null; } }

        public virtual IResource Parent { get; private set; }

        public virtual ResourceCollection children { get; private set; }

        #endregion        

        #region PropertyAliases

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

        [ResourceProperty(CommonResourceProperty.GUID, ResourcePropertyGroup.General, IsReadOnly = true)]
        public string GUIDName
        {
            get
            {
                return GUID.ToString("b");
            }
        }
        #endregion
    }
}
