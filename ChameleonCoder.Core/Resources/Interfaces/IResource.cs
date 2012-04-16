using System.ComponentModel;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to be implemented by resource types
    /// </summary>
    public interface IResource : INotifyPropertyChanged, IComponent
    {
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
        XmlElement Xml { get; }

        /// <summary>
        /// will contain the file in which the resource is defined
        /// </summary>
        Files.DataFile File { get; }

        /// <summary>
        /// holds a reference to the resource's parent
        /// </summary>
        IResource Parent { get; }

        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        /// <remarks>IMPORTANT: Implementations MUST register to this member's <c>CollectionChanged</c> event
        /// and raise the <c>PropertyChanged</c> event in response to it!</remarks>
        ResourceCollection Children { get; }

        /// <summary>
        /// a collection containing the references this resource contains
        /// </summary>
        /// <remarks>IMPORTANT: Implementations MUST register to this member's <c>CollectionChanged</c> event
        /// and raise the <c>PropertyChanged</c> event in response to it!</remarks>
        ReferenceCollection References { get; }

        /// <summary>
        /// updates the resource
        /// </summary>
        /// <param name="data">the XmlNode containing the resource data.</param>
        /// <param name="parent">the parent resource for the resource</param>
        void Update(XmlElement data, IResource parent);
    }
}
