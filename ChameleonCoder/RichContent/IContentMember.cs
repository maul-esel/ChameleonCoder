using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;

namespace ChameleonCoder.RichContent
{
    public interface IContentMember
    {
        /// <summary>
        /// the alias thnat is used as key-name in XML
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// the list of childMembers the content member has
        /// </summary>
        List<IContentMember> childMembers { get; }

        /// <summary>
        /// asks the content member for permission to add a child to its childMember list.
        /// In this way, a function may allow a ParameterMember object but refuse a ClassMember object.
        /// THIS FUNCTION SHOULD NOT ADD IT TO THE LIST!
        /// </summary>
        /// <param name="child">the child to check</param>
        /// <returns>true if the child would be accepted, false otherwise</returns>
        bool ValidateChild(IContentMember child);

        /// <summary>
        /// adds a child to the childMember collection.
        /// The content member should not validate it, as this is done using Validate()
        /// </summary>
        /// <param name="child">the child to add</param>
        void AddChild(IContentMember child);

        /// <summary>
        /// a language that is required for the content member.
        /// This is intended for language-specific types such as "XmlNode" or "IniSection".
        /// To allow all languages, use Guid.Empty.
        /// </summary>
        Guid RequiredLanguage { get; }
    }
}
