using System;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.Resources
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ResourcePropertyAttribute : Attribute
    {
        public ResourcePropertyAttribute(string name, ResourcePropertyGroup group)
        {
            this.name = name;
            this.group = group;
        }

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

        internal string Name { get { return name; } }

        internal ResourcePropertyGroup Group { get { return group; } }

        public bool IsReadOnly { get; set; }

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

    public enum ResourcePropertyGroup
    {
        General,
        ThisClass,
        CurrentClass
    }

    public enum CommonResourceProperty
    {
        Identifier,
        Name,
        Description,
        Parent,
        CompilationPath,
        FSPath,
        Language,
        CompatibleLanguages
    }
}
