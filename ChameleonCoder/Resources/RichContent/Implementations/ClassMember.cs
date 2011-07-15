using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources.RichContent.Implementations
{
    public class ClassMember : IContentMember
    {
        [Obsolete]
        public virtual string Alias { get { return null; } }

        public List<IContentMember> childMembers { get; set; }

        [Obsolete]
        public virtual Guid RequiredLanguage { get; set; }

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
