using System;

namespace ChameleonCoder.Resources.RichContent
{
    internal sealed class ContentMemberCollection : ComponentCollection<string, Type>
    {
        internal void RegisterMember(ContentMemberTypeInfo info, Type member)
        {
            base.RegisterComponent(info.Alias, member);
        }

        internal Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
