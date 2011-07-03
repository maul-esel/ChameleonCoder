using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources
{
    public interface ILanguageResource
    {
        Guid language { get; }
        List<Guid> compatibleLanguages { get; }
    }
}
