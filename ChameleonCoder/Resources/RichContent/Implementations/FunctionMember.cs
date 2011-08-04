using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources.RichContent.Implementations
{
    public class FunctionMember : IContentMember
    {
        #region IContentMember

        public virtual bool ValidateChild(IContentMember child)
        {
            if (child is ParameterMember || child is ReturnValueMember)
                return true;
            return false;
        }

        public virtual bool ValidateParent(IContentMember parent)
        {
            return false;
        }

        public virtual void AddChild(IContentMember child)
        {
            this.childMembers.Add(child);
        }

        public RichContentCollection childMembers
        {
            get;
            private set;
        }

        public virtual void Save() { }
        public virtual void Init(System.Xml.XmlNode node) { }

        #endregion

        public FunctionMember()
        {
            this.childMembers = new RichContentCollection();
        }
    }
}
