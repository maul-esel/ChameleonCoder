using System;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    public class FunctionMember : IContentMember
    {
        #region IContentMember

        public string GetHtml(object param)
        {
            return string.Empty;
        }

        public RichContentCollection Children
        {
            get;
            private set;
        }

        public virtual void Save() { }

        public IContentMember Parent { get; protected set; }

        public string Name { get; protected set; }

        public Guid Identifier { get; protected set; }

        public System.Windows.Media.ImageSource Icon { get; private set; }

        public virtual void Initialize(System.Xml.XmlElement node, IContentMember parent) { }

        #endregion

        public FunctionMember()
        {
            this.Children = new RichContentCollection();
        }
    }
}
