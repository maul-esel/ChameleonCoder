using System;
using System.Windows.Media;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource : System.Collections.Generic.IEnumerable<Resources.Base.PropertyDescription>
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
        /// the associated metadata as Metadata class instances
        /// </summary>
        Collections.MetadataCollection MetaData { get; }

        /// <summary>
        /// an image representing an important information related to the resource
        /// </summary>
        ImageSource SpecialVisualProperty { get; }

        /// <summary>
        /// contains the XmlNode representing the resource
        /// Any changes to the resource should be immediately saved to this XmlNode.
        /// </summary>
        System.Xml.XmlNode Xml { get; }

        /// <summary>
        /// saves the information changed through the UI to the XML representation
        /// </summary>
        void Save();

        /// <summary>
        /// validates whether a RichContent member can be applied to this resource or not
        /// </summary>
        /// <param name="member">the RichContentMember to validate</param>
        /// <returns>true if the member can be added, false otherwise</returns>
        bool ValidateRichContent(RichContent.IContentMember member);

        /// <summary>
        /// adds a metadata element, given any changes through the UI
        /// it directly manipulates
        /// 1) the Xml-Document
        /// 2) the MetadataCollection
        /// </summary>
        void AddMetadata();

        void DeleteMetadata();

        /// <summary>
        /// moves a resource to another parent resource
        /// </summary>
        void Move();

        /// <summary>
        /// deletes a resource
        /// </summary>
        void Delete();
    }
}
