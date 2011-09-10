using System;
using System.Collections.Generic;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface ILanguageResource : IResource
    {
        Guid language { get; }
        List<Guid> compatibleLanguages { get; } // todo: replace by 'IList' or even 'IEnumerable'
    }
}
