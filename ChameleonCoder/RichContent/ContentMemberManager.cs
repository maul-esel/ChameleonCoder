using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;

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

            var externMembers = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Plugins", "*.dll")
                         let assembly = Assembly.LoadFrom(dll)
                         from type in assembly.GetTypes()
                         where type.GetInterface(typeof(IContentMember).FullName) != null
                         select type;

            var members = internMembers.Concat(externMembers);

            foreach (Type member in members)
            {
                ContentMembers.RegisterMember(member);
            }
        }
    }
}
