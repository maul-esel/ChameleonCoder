using System;

namespace ChameleonCoder.RichContent
{
    public class ContentMemberCollection : ComponentCollection<string, IContentMember>
    {
        public void RegisterMember(IContentMember member)
        {
            base.RegisterComponent(member.Alias, member);
        }

        public IContentMember GetMember(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
