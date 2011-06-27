using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent
{
    interface ICodeMember : IContentMember
    {
        string CodeExample { get; }
    }
}
