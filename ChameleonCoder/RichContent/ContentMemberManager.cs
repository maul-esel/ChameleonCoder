using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent
{
    public class ContentMemberManager : ComponentManager<string, IContentMember>
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
