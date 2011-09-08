using System;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.Resources
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ResourcePropertyAttribute : Attribute
    {
        public ResourcePropertyAttribute(string name, ResourcePropertyGroup group)
        {
            Name = name;
            Group = group;
        }

        public ResourcePropertyAttribute(CommonResourceProperty property, ResourcePropertyGroup group)
        {
            Group = group;
            switch (property)
            {
                case CommonResourceProperty.GUID: Name = GUID_Name; break;
                case CommonResourceProperty.Name: Name = Name_Name; break;
                case CommonResourceProperty.Description: Name = Description_Name; break;
                case CommonResourceProperty.Parent: Name = Parent_Name; break;
                case CommonResourceProperty.CompilationPath: Name = CompilationPath_Name; break;
                case CommonResourceProperty.FSPath: Name = FSPath_Name; break;
                case CommonResourceProperty.Language: Name = Language_Name; break;
                case CommonResourceProperty.CompatibleLanguages: Name = CompatibleLanguages_Name; break;
                default: Name = string.Empty; break;
            }
        }

        internal readonly string Name;
        internal readonly ResourcePropertyGroup Group;

        public bool IsReadOnly { get; set; }
        public bool IsReferenceName { get; set; }

        #region Names

        private static string GUID_Name { get { return Res.Info_GUID; } }

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
        GUID,
        Name,
        Description,
        Parent,
        CompilationPath,
        FSPath,
        Language,
        CompatibleLanguages
    }
}
