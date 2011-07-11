using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent.Implementations
{
    public class FunctionMember : IContentMember
    {
        #region IContentMember

        virtual bool IContentMember.ValidateChild(IContentMember child)
        {
            if (child is ParameterMember || child is ReturnValueMember)
                return true;
            return false;
        }

        virtual bool IContentMember.ValidateParent(IContentMember parent)
        {
            return false;
        }

        virtual void IContentMember.AddChild(IContentMember child)
        {
            this.childMembers.Add(child);
        }

        virtual Guid IContentMember.RequiredLanguage { get { return Guid.Empty; } }

        public List<IContentMember> childMembers
        {
            get;
            private set;
        }

        virtual string IContentMember.Alias { get { return "function"; } }

        #endregion

        public FunctionMember()
        {
            this.childMembers = new List<IContentMember>();
        }
    }
}
