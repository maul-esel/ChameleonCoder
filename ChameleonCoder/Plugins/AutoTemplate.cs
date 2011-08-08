﻿using System;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Plugins
{
    internal sealed class AutoTemplate : ITemplate
    {
        internal AutoTemplate(Type resourceType, ResourceTypeInfo info)
        {
            this.info = info;
            ResourceType = resourceType;
        }

        ResourceTypeInfo info;
        Guid instanceId = Guid.NewGuid();

        #region IComponent
        public string About
        {
            get { return "© 2011: auto-generated ChameleonCoder Template\n" + info.Author; }
        }

        public string Author
        {
            get { return string.Format(Properties.Resources.AutoTemplate_Author, info.Author); }
        }

        public string Description
        {
            get { return string.Format(Properties.Resources.AutoTemplate_Description, info.DisplayName); }
        }

        public System.Windows.Media.ImageSource Icon
        {
            get { return info.TypeIcon; }
        }

        public Guid Identifier
        {
            get { return instanceId; }
        }

        public string Name
        {
            get { return info.DisplayName; }
        }

        public string Version
        {
            get { return "0.0.0.1"; }
        }

        public void Initialize() { }

        public void Shutdown() { }
        #endregion

        #region ITemplate

        public string DefaultName
        {
            get
            {
                TemplateDefaultNameAttribute attr = (TemplateDefaultNameAttribute)Attribute.GetCustomAttribute(ResourceType, typeof(TemplateDefaultNameAttribute));
                if (attr != null)
                    return attr.Name;
                return info.DisplayName + i;
            }
        }

        int i = 1;

        public string Group
        {
            get { return null; }
        }

        public Type ResourceType
        {
            get;
            private set;
        }

        public Resources.Interfaces.IResource Create(IResource parent, string name)
        {
            return info.Create(ResourceType, parent, name);
        }
        #endregion
    }
}
