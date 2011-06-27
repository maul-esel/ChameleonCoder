using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent
{
    class FunctionMember : ICodeMember
    {
        #region ICodeMember

        bool IContentMember.ValidateChild(IContentMember child)
        {
            return false;
        }

        void IContentMember.AddChild(IContentMember child)
        {
            this.childMembers.Add(child);
        }

        Guid IContentMember.RequiredLanguage { get { return Guid.Empty; } }

        public List<IContentMember> childMembers
        {
            get;
            private set;
        }

        string IContentMember.Alias { get { return "function"; } }

        string ICodeMember.CodeExample { get { return _code; } }

        #endregion

        string _code;

        FunctionMember()
        {
            this.childMembers = new List<IContentMember>();
        }
    }
}
