using System;

namespace ChameleonCoder.RichContent
{
    public class ContentMemberCollection : ComponentCollection<string, Type>
    {
        public void RegisterMember(string alias, Type member)
        {
            base.RegisterComponent(alias, member);
        }

        public Type GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
