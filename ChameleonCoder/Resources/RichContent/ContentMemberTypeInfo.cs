using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources.RichContent
{
    public class ContentMemberTypeInfo
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
