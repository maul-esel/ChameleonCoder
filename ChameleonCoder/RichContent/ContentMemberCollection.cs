using System;

namespace ChameleonCoder.RichContent
{
    internal sealed class ContentMemberCollection : ComponentCollection<string, Type>
    {
        internal void RegisterMember(string alias, Type member)
        {
            base.RegisterComponent(alias, member);
        }

        internal Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
