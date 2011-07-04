using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources
{
    public interface IResource
    {
        string Alias { get; }

        string DisplayName { get; }

        Guid GUID { get; }

        void Save();
    }
}
