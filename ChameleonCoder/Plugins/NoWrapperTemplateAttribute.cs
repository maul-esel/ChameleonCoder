﻿using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines that a wrapper template should not be created for this class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NoWrapperTemplateAttribute : Attribute
    {
    }
}
