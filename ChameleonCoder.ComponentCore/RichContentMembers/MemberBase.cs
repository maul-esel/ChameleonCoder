using System;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// serves as base class for RichContent types
    /// </summary>
    public abstract class MemberBase : IContentMember
    {
        /// <summary>
        /// a base constructor for inherited types
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        protected MemberBase(XmlElement data, IContentMember parent, IResource resource)
        {
            parentMember = parent;
            xmlData = data;

            Identifier = Guid.Parse(data.GetAttribute("id", DataFile.NamespaceUri));
        }

        private readonly IContentMember parentMember;

        private readonly XmlElement xmlData;

        private readonly RichContentCollection childrenCollection = new RichContentCollection();


        protected XmlElement Xml { get { return xmlData; } }


        public IContentMember Parent { get { return parentMember; } }

        public RichContentCollection Children { get { return childrenCollection; } }

        public string Name
        {
            get
            {
                return xmlData.GetAttribute("name", DataFile.NamespaceUri);
            }
            set
            {
                xmlData.SetAttribute("name", DataFile.NamespaceUri, value);
            }
        }

        public Guid Identifier { get; private set; }

        public virtual System.Windows.Media.ImageSource Icon { get { return null; } }


        public abstract void Save();

        public abstract string GetHtml(object data);

    }
}
