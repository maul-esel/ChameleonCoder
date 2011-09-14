using System;

namespace ChameleonCoder.Resources.RichContent
{
    internal sealed class ContentMemberCollection : ComponentCollection<string, Type>
    {
        internal void RegisterContentMember(string alias, Type member)
        {
            base.RegisterComponent(alias, member);
        }

        internal Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
