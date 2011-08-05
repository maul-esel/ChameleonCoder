using System;

namespace ChameleonCoder
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class CCPluginAttribute : Attribute
    {
        public CCPluginAttribute(string pluginName, string author, string version)
        {
            Name = pluginName;
            Author = author;
            Version = version;
        }

        public readonly string Name;
        public readonly string Author;
        public readonly string Version;
    }
}
