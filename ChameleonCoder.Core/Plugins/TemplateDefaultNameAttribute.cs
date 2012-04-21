using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines a Custom default name for a resource created through the wrapper template
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), System.Runtime.InteropServices.ComVisible(false)]
    public sealed class TemplateDefaultNameAttribute : Attribute
    {
        /// <summary>
        /// creates a new instance of the attribute
        /// </summary>
        /// <param name="name">the default name to use</param>
        public TemplateDefaultNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// gets the name to use as default name by automatically created templates
        /// </summary>
        public string Name { get; private set; }
    }
}
