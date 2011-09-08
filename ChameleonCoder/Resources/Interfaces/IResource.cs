using System;
using System.Windows.Media;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// the resource's icon in the main treeview
        /// </summary>
        ImageSource Icon { get; }
        
        /// <summary>
        /// the unique identifier
        /// </summary>
        Guid GUID { get; }

        /// <summary>
        /// the user-defined name of the resource
        /// Mutilple resources with the same name are allowed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// a user-defined short description of the resource
        /// </summary>
        string Description { get; }

        /// <summary>
        /// any user-defined notes related to the resource
        /// </summary>
        string Notes { get; }

        /// <summary>
        /// an image representing an important information related to the resource
        /// </summary>
        ImageSource SpecialVisualProperty { get; }

        /// <summary>
        /// contains the XmlNode representing the resource
        /// Any changes to the resource should be immediately saved to this XmlNode.
        /// </summary>
        System.Xml.XmlElement Xml { get; }

        /// <summary>
        /// holds a reference to the resource's parent
        /// </summary>
        IResource Parent { get; }

        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        ResourceCollection children { get; }

        /// <summary>
        /// initializes the resource
        /// </summary>
        /// <param name="data">the XmlNode containing the resource data.</param>
        /// <param name="parent">the parent resource for the resource</param>
        void Init(System.Xml.XmlElement data, IResource parent);
    }
}
