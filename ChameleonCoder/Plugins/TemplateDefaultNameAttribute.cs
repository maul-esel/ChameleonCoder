using System;

namespace ChameleonCoder.Plugins
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateDefaultNameAttribute : Attribute
    {
        public TemplateDefaultNameAttribute(string name)
        {
            Name = name;
        }

        internal readonly string Name;
    }
}
