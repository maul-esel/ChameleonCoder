using System;
using System.Reflection;
using System.IO;
using System.Linq;

namespace ChameleonCoder.RichContent
{
    internal static class ContentMemberManager
    {
        private static ContentMemberCollection ContentMembers = new ContentMemberCollection();

        internal static void Load()
        {
            var internMembers = from type in Assembly.GetEntryAssembly().GetTypes()
                         where type.GetInterface(typeof(IContentMember).FullName) != null
                         select type;

            var externMembers = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Components", "*.dll")
                         let assembly = Assembly.LoadFrom(dll)
                         from type in assembly.GetTypes()
                         where type.GetInterface(typeof(IContentMember).FullName) != null
                         select type;

            var members = internMembers.Concat(externMembers);

            foreach (Type member in members)
            {
                if (!member.IsAbstract && !member.IsInterface && !member.IsNotPublic)
                    ContentMembers.RegisterMember(member);
            }
        }
    }
}
