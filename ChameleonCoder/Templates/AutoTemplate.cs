using System;
using System.Collections.Generic;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Templates
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
            get { return "(c) 2011: auto-generated ChameleonCoder Template\noriginal author: " + info.Author; }
        }

        public string Author
        {
            get { return "ChameleonCoder (original author: " + info.Author + ")"; }
        }

        public string Description
        {
            get { return "auto-enerated wrapper template for the " + info.DisplayName + " resource type."; }
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
            get { return info.DisplayName + "-template"; }
        }

        public string Version
        {
            get { return "0.0.0.1"; }
        }

        public void Initialize() { }

        public void Shutdown() { }
        #endregion

        #region ITemplate
        public Type ResourceType { get; private set; }

        public string Group
        {
            get { return null; }
        }

        public IEnumerable<Guid> Languages
        {
            get { return null; }
        }

        public Resources.Interfaces.IResource Create(Resources.Interfaces.IResource parent)
        {
            return info.Create(ResourceType, parent);
        }
        #endregion
    }
}
