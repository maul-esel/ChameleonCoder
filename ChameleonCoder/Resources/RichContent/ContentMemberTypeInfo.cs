using System;
using System.Windows.Media;

namespace ChameleonCoder.Resources.RichContent
{
    [Obsolete("use component factory", false)]
    public sealed class ContentMemberTypeInfo
    {
        public ContentMemberTypeInfo(string alias, Guid requiredLang)
        {
            this.Alias = alias;
            this.RequiredLanguage = requiredLang;
        }

        public string Alias { get; private set; }

        public Guid RequiredLanguage { get; private set; }

        public ImageSource TypeIcon { get; private set; }
    }
}
