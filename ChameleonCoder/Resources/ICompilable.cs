﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Resources
{
    public interface ICompilable : ILanguageResource
    {
        string compilationPath { get; }
    }
}
