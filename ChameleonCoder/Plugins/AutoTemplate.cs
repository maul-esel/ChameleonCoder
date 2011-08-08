﻿using System;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// provides a wrapper-implementation of ITemplate for resource types
    /// </summary>
    internal sealed class AutoTemplate : ITemplate
    {
        /// <summary>
        /// creates a new instance of the AutoTemplate class
        /// </summary>
        /// <param name="resourceType">the resource type to wrap</param>
        /// <param name="info">the ResourceTypeInfo corresponding to the resource type</param>
        internal AutoTemplate(Type resourceType, ResourceTypeInfo info)
        {
            this.info = info;
            ResourceType = resourceType;
        }

        ResourceTypeInfo info;
        Guid instanceId = Guid.NewGuid();

        #region IPlugin
        /// <summary>
        /// gives information about copyright etc.
        /// </summary>
        public string About
        {
            get { return "© 2011: auto-generated ChameleonCoder Template\n" + info.Author; }
        }

        /// <summary>
        /// the template author, in this case a localized information
        /// </summary>
        public string Author
        {
            get { return string.Format(Properties.Resources.AutoTemplate_Author, info.Author); }
        }

        /// <summary>
        /// the template description, in this case a localized information
        /// </summary>
        public string Description
        {
            get { return string.Format(Properties.Resources.AutoTemplate_Description, info.DisplayName); }
        }

        /// <summary>
        /// the template icon, in this case the resource type's TypeIcon
        /// </summary>
        public System.Windows.Media.ImageSource Icon
        {
            get { return info.TypeIcon; }
        }

        /// <summary>
        /// the template identifier
        /// </summary>
        public Guid Identifier
        {
            get { return instanceId; }
        }

        /// <summary>
        /// the template name, in this case the resource type's name
        /// </summary>
        public string Name
        {
            get { return info.DisplayName; }
        }

        /// <summary>
        /// the template version
        /// </summary>
        public string Version
        {
            get { return "0.0.0.1"; }
        }

        /// <summary>
        /// initializes the template
        /// </summary>
        public void Initialize() { }

        /// <summary>
        /// prepares the template for shutdown
        /// </summary>
        public void Shutdown() { }

        #endregion

        #region ITemplate

        /// <summary>
        /// the default name for a resource created through this template,
        /// in this case the resoure type's DisplayName and an indexing number.
        /// If the TemplateDefaultName attribute is applied to the type, its Name is used instead.
        /// </summary>
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

        /// <summary>
        /// the group for this template, in this case null.
        /// </summary>
        public string Group
        {
            get { return null; }
        }

        /// <summary>
        /// the resource type the template creates
        /// </summary>
        public Type ResourceType
        {
            get;
            private set;
        }

        /// <summary>
        /// creates a new instance of the resource type.
        /// </summary>
        /// <param name="parent">the parent resource or null</param>
        /// <param name="name">the name for the resource</param>
        /// <returns>the new resource</returns>
        public IResource Create(IResource parent, string name)
        {
            return info.Create(ResourceType, parent, name);
        }
        #endregion
    }
}
