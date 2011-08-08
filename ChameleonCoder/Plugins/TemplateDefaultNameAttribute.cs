using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines a custom default name for a resource created through the wrapper template
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateDefaultNameAttribute : Attribute
    {
        /// <summary>
        /// creates a new instance of the attribute
        /// </summary>
        /// <param name="name">the default name to use</param>
        public TemplateDefaultNameAttribute(string name)
        {
            Name = name;
        }

        internal readonly string Name;
    }
}
