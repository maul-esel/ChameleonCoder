using System;
using System.Reflection;
using System.IO;
using System.Linq;

namespace ChameleonCoder.RichContent
{
    internal static class ContentMemberManager
    {
        private static ContentMemberCollection ContentMembers = new ContentMemberCollection();

        internal static void RegisterComponent(Type component, string alias)
        {
            if (component.GetInterface(typeof(IContentMember).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic)
            {
                ContentMembers.RegisterMember(alias, component);
            }
        }
    }
}
