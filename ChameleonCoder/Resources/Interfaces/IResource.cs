using System;
using System.Windows.Media;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource : System.Collections.IEnumerable
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
        MetadataCollection MetaData { get; }

        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        ResourceCollection children { get; }

        // a RichContent collection

        /// <summary>
        /// an image representing an important information related to the resource
        /// </summary>
        ImageSource SpecialVisualProperty { get; }

        /// <summary>
        /// saves the information changed through the UI to the XML representation
        /// </summary>
        void Save();

        /// <summary>
        /// creates a new instance of the resource type + saves it to XML
        /// </summary>
        /// <returns></returns>
        IResource Create();

        /// <summary>
        /// adds a new RichContentMember to the resource
        /// the resource can validate it beforehand and is "allowed" to refuse
        /// return false in such cases
        /// </summary>
        /// <param name="member">the RichContentMember to add</param>
        /// <returns>true if the member was added, false otherwise</returns>
        bool AddRichContentMember(ChameleonCoder.RichContent.IContentMember member);

        /// <summary>
        /// attaches a new resource to the running instance
        /// </summary>
        void AttachResource(IResource newChild);

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
