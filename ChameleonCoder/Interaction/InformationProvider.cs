﻿using System;

namespace ChameleonCoder.Interaction
{
    public static class InformationProvider
    {
        public static string ProgrammingDirectory { get { return Properties.Settings.Default.ProgrammingDir; } }
    }
}
