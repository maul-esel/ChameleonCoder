using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;

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
        /// <param name="data">a dictionary that contains the resource attributes</param>
        /// <param name="parent">the parent resource</param>
        public virtual void Update(ObservableStringDictionary data, IResource parent, IDataFile file)
        {
            File = file;
            Attributes = data;
            Parent = parent;

            Guid id;
            string guid = data["id"];
            if (!Guid.TryParse(guid, out id))
            {
                id = Guid.NewGuid();
                data["id"] = id.ToString("b");
            }
            Identifier = id;
        }

        public virtual void AddChild(IResource child)
        {
            childrenCollection.Add(child);
            OnPropertyChanged("Children");
        }

        public virtual void RemoveChild(IResource child)
        {
            childrenCollection.Remove(child);
            OnPropertyChanged("Children");
        }

        public virtual void AddReference(ResourceReference reference)
        {
            referenceCollection.Add(reference);
            OnPropertyChanged("References");
        }

        public virtual void RemoveReference(ResourceReference reference)
        {
            referenceCollection.Remove(reference);
            OnPropertyChanged("References");
        }

        /// <summary>
        /// gets the XmlElement representing the resource
        /// </summary>
        /// <value>This value is the XmlElement given to the resource in the <see cref="Update"/> method.</value>
        [Obsolete]
        public XmlElement Xml { get; private set; }

        public ObservableStringDictionary Attributes
        {
            get;
            private set;
        }

        /// <summary>
        /// when overriden in a derived class, gets the resource's icon
        /// </summary>
        public abstract ImageSource Icon { get; }

        /// <summary>
        /// gets the resource's unique identifier
        /// </summary>
        public Guid Identifier { get; protected set; }

        public IDataFile File { get; protected set; }

        /// <summary>
        /// gets a list containing the resource's references
        /// </summary>
        public ResourceReference[] References
        {
            get { return referenceCollection.Values; }
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
                return Attributes["name"]; // Xml.GetAttribute("name", DataFile.NamespaceUri);
            }
            set
            {
                Attributes["name"] = value; // Xml.SetAttribute("name", DataFile.NamespaceUri, value);
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
                return Attributes["description"]; // Xml.GetAttribute("description", DataFile.NamespaceUri);
            }
            set
            {
                Attributes["description"] = value; // Xml.SetAttribute("description", DataFile.NamespaceUri, value);
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
                return Attributes["notes"]; // Xml.GetAttribute("notes", DataFile.NamespaceUri);
            }
            set
            {
                Attributes["notes"] = value; // Xml.SetAttribute("notes", DataFile.NamespaceUri, value);
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
        public IResource[] Children
        {
            get { return childrenCollection.Values; }
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
