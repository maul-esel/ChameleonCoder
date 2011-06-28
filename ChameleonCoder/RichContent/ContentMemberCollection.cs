using System;

namespace ChameleonCoder.RichContent
{
    internal sealed class ContentMemberCollection : ComponentCollection<string, Type>
    {
        internal void RegisterMember(Type member)
        {
            base.RegisterComponent((Activator.CreateInstance(member) as IContentMember).Alias, member);
        }

        internal Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
