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
            Children = children;
            Type = type;
        }

        internal ContextType Type { get; private set; }

        public IEnumerable Children { get; private set; }

        public ImageSource Icon { get; private set; }

        public string Name { get; private set; }

        internal enum ContextType
        {
            Home,
            ResourceList,
            Settings,
            Plugins            
        }
    }    
}
