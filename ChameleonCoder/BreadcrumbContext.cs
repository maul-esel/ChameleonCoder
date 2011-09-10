using System.Collections;
using System.Windows.Media;

namespace ChameleonCoder
{
    internal sealed class BreadcrumbContext
    {
        internal BreadcrumbContext(string name, ImageSource icon, IEnumerable children, ContextType type)
        {
            Name = name;
            Icon = icon;
            this.children = children;
            Type = type;
        }

        internal ContextType Type { get; private set; }

        internal IEnumerable children { get; private set; }

        internal ImageSource Icon { get; private set; }

        internal string Name { get; private set; }

        internal enum ContextType
        {
            ResourceList,
            Settings,
            Plugins,
            Home
        }
    }    
}
