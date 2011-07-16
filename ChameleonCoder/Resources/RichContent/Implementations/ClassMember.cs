using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources.RichContent.Implementations
{
    public class ClassMember : IContentMember
    {
        public RichContentCollection childMembers { get; set; }

        public virtual bool ValidateChild(IContentMember child)
        {
            return false;
        }

        public virtual bool ValidateParent(IContentMember parent)
        {
            return false;
        }

        public virtual void AddChild(IContentMember child)
        {

        }
    }
}
