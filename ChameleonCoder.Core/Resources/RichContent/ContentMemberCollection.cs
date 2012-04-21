using System;

namespace ChameleonCoder.Resources.RichContent
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class ContentMemberCollection : ComponentCollection<Guid, Type>
    {
        internal void RegisterContentMember(Guid key, Type member)
        {
            base.RegisterComponent(key, member);
        }

        internal Type GetMember(Guid key)
        {
            return base.GetComponent(key);
        }
    }
}
