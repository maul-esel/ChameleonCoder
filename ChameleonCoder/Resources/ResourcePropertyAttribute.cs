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

        public ResourcePropertyAttribute(CommonResourceProperty name, ResourcePropertyGroup group)
        {
            Group = group;

            string _name = string.Empty;
            switch (name)
            {
                case CommonResourceProperty.GUID: _name = GUID_Name; break;
                case CommonResourceProperty.Name: _name = Name_Name; break;
                case CommonResourceProperty.Description: _name = Description_Name; break;
                case CommonResourceProperty.Parent: _name = Parent_Name; break;
                case CommonResourceProperty.CompilationPath: _name = CompilationPath_Name; break;
                case CommonResourceProperty.FSPath: _name = FSPath_Name; break;
                case CommonResourceProperty.Language: _name = Language_Name; break;
                case CommonResourceProperty.CompatibleLanguages: _name = CompatibleLanguages_Name; break;
            }
            Name = _name;
        }

        public readonly string Name;
        public readonly ResourcePropertyGroup Group;

        public bool IsReadOnly { get; set; }
        public bool IsReferenceName { get; set; }

        #region Names

        public static string GUID_Name { get { return Res.Info_GUID; } }

        public static string Name_Name { get { return Res.Info_Name; } }

        public static string Description_Name { get { return Res.Info_Description; } }

        public static string Parent_Name { get { return Res.Info_Parent; } }

        public static string CompilationPath_Name { get { return Res.Info_CompilationPath; } }

        public static string FSPath_Name { get { return Res.Info_FSPath; } }

        public static string Language_Name { get { return Res.Info_Language; } }

        public static string CompatibleLanguages_Name { get { return Res.Info_CompatibleLanguages; } }

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
