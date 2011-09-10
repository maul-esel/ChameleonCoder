using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines a Custom default name for a resource created through the wrapper template
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TemplateDefaultNameAttribute : Attribute
    {
        /// <summary>
        /// creates a new instance of the attribute
        /// </summary>
        /// <param name="name">the default name to use</param>
        public TemplateDefaultNameAttribute(string name)
        {
            this.name = name;
        }

        internal string Name { get { return name; } }

        private readonly string name;
    }
}
