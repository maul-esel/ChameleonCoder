using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource : IEnumerable<PropertyDescription>
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
        /// an image representing an important information related to the resource
        /// </summary>
        ImageSource SpecialVisualProperty { get; }

        /// <summary>
        /// contains the XmlNode representing the resource
        /// Any changes to the resource should be immediately saved to this XmlNode.
        /// </summary>
        System.Xml.XmlNode Xml { get; }

        /// <summary>
        /// holds a reference to the resource's parent
        /// </summary>
        IResource Parent { get; }

        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        ResourceCollection children { get; }
    }
}
