using System;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// an attribute to be applied to properties in a resource type
    /// </summary>
    /// <remarks>All properties that have this attribute applied will be shown in the resource view.</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ResourcePropertyAttribute : Attribute
    {
        /// <summary>
        /// creates a new instance of the attribute
        /// </summary>
        /// <param name="name">the name of the property. See also <see cref="IsReferenceName"/>.</param>
        /// <param name="group">the group in which to place the property</param>
        public ResourcePropertyAttribute(string name, ResourcePropertyGroup group)
        {
            this.name = name;
            this.group = group;
        }

        /// <summary>
        /// creates a new instance of the attribute
        /// </summary>
        /// <param name="property">the common property value</param>
        /// <param name="group">the group in which to place the property</param>
        public ResourcePropertyAttribute(CommonResourceProperty property, ResourcePropertyGroup group)
        {
            this.group = group;
            switch (property)
            {
                case CommonResourceProperty.Identifier: name = Identifier_Name; break;
                case CommonResourceProperty.Name: name = Name_Name; break;
                case CommonResourceProperty.Description: name = Description_Name; break;
                case CommonResourceProperty.Parent: name = Parent_Name; break;
                case CommonResourceProperty.CompilationPath: name = CompilationPath_Name; break;
                case CommonResourceProperty.FSPath: name = FSPath_Name; break;
                case CommonResourceProperty.Language: name = Language_Name; break;
                case CommonResourceProperty.CompatibleLanguages: name = CompatibleLanguages_Name; break;
                default: name = string.Empty; break;
            }
        }

        private readonly string name;
        private readonly ResourcePropertyGroup group;

        /// <summary>
        /// gets the name of the property
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// gets the group of the property
        /// </summary>
        public ResourcePropertyGroup Group { get { return group; } }

        /// <summary>
        /// gets or sets whether the property is read-only.
        /// </summary>
        /// <remarks>If there's no public set-accessor, the property will be read-only anyway.</remarks>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Sets whether the given name is the actual name or just a reference to another property
        /// </summary>
        /// <remarks>You can set the "name" parameter to be the name of a static or instance-property in your
        /// class. If this property has a public get-accessor, its value will be taken as the actual name.</remarks>
        public bool IsReferenceName { get; set; }

        #region Names

        private static string Identifier_Name { get { return Res.Info_Identifier; } }

        private static string Name_Name { get { return Res.Info_Name; } }

        private static string Description_Name { get { return Res.Info_Description; } }

        private static string Parent_Name { get { return Res.Info_Parent; } }

        private static string CompilationPath_Name { get { return Res.Info_CompilationPath; } }

        private static string FSPath_Name { get { return Res.Info_FSPath; } }

        private static string Language_Name { get { return Res.Info_Language; } }

        private static string CompatibleLanguages_Name { get { return Res.Info_CompatibleLanguages; } }

        #endregion
    }

    /// <summary>
    /// an enumeration defining in which property group to place a property
    /// with the <see cref="ChameleonCoder.Resources.ResourcePropertyAttribute"/> applied on it.
    /// </summary>
    public enum ResourcePropertyGroup
    {
        /// <summary>
        /// place the property in the "General" group.
        /// </summary>
        /// <remarks>You should only use this for properties
        /// corresponding to <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> properties.</remarks>
        General,

        /// <summary>
        /// the display name of the class declaring the property will be used
        /// </summary>
        /// <remarks>If the declaring type is not registered,
        /// the <see cref="CurrentClass"/> value will be used instead.</remarks>
        ThisClass,

        /// <summary>
        /// the display name of the class the resource is an instance of will be used.
        /// </summary>
        CurrentClass
    }

    /// <summary>
    /// defines common properties of resource types
    /// </summary>
    public enum CommonResourceProperty
    {
        /// <summary>
        /// the unique identifier of the resource
        /// </summary>
        Identifier,

        /// <summary>
        /// the name of the resource
        /// </summary>
        Name,

        /// <summary>
        /// the resource's description
        /// </summary>
        Description,

        /// <summary>
        /// the resource's parent
        /// </summary>
        Parent,

        /// <summary>
        /// the resource's compilation path (for <see cref="ChameleonCoder.Resources.Interfaces.ICompilable"/> resources)
        /// </summary>
        CompilationPath,

        /// <summary>
        /// the resource's file system path (for <see cref="ChameleonCoder.Resources.Interfaces.IFSComponent"/> resources)
        /// </summary>
        FSPath,

        /// <summary>
        /// the resource's coding language (for <see cref="ChameleonCoder.Resources.Interfaces.ILanguageResource"/> resources)
        /// </summary>
        Language,

        /// <summary>
        /// the resource's compatible languages (for <see cref="ChameleonCoder.Resources.Interfaces.ILanguageResource"/> resources)
        /// </summary>
        CompatibleLanguages
    }
}
