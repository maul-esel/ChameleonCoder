using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IEditable
    {
        string GetText();

        void SaveText(string text);
    }
}
