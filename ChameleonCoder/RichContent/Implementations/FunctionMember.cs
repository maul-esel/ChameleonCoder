using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent.Implementations
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

        public virtual Guid RequiredLanguage { get { return Guid.Empty; } }

        public List<IContentMember> childMembers
        {
            get;
            private set;
        }

        public virtual string Alias { get { return "function"; } }

        #endregion

        public FunctionMember()
        {
            this.childMembers = new List<IContentMember>();
        }
    }
}
