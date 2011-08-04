using System;
using System.Collections.Generic;

namespace ChameleonCoder.Resources.RichContent
{
    public interface IContentMember
    {
        /// <summary>
        /// the list of childMembers the content member has
        /// </summary>
        RichContentCollection childMembers { get; }

        /// <summary>
        /// asks the content member for permission to add a child to its childMember list.
        /// In this way, a function may allow a ParameterMember object but refuse a ClassMember object.
        /// THIS FUNCTION SHOULD NOT ADD IT TO THE LIST!
        /// </summary>
        /// <param name="child">the child to check</param>
        /// <returns>true if the child would be accepted, false otherwise</returns>
        bool ValidateChild(IContentMember child);

        bool ValidateParent(IContentMember parent);

        void Save();
        void Init(System.Xml.XmlNode node);
    }
}
