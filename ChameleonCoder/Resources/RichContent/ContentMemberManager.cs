using System;
using System.Reflection;
using System.IO;
using System.Linq;

namespace ChameleonCoder.Resources.RichContent
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

        internal static IContentMember CreateInstanceOf(string alias, params object[] parameters)
        {
            Type member = ContentMembers.GetMember(alias);
            if (member == null)
                return null;
            return (IContentMember)Activator.CreateInstance(member, parameters);
        }
    }
}
