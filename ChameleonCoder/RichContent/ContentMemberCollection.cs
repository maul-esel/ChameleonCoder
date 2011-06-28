using System;

namespace ChameleonCoder.RichContent
{
    public class ContentMemberCollection : ComponentCollection<string, Type>
    {
        public void RegisterMember(Type member)
        {
            base.RegisterComponent((Activator.CreateInstance(member) as IContentMember).Alias, member);
        }

        public Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
