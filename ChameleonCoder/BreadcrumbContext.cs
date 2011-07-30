using System;
using System.Collections;
using System.Linq;
using System.Windows.Media;
using System.Text;

namespace ChameleonCoder
{
    class BreadcrumbContext
    {
        internal BreadcrumbContext(string name, ImageSource icon, IEnumerable children)
            : this(name, icon, children, false, false)
        {
        }

        internal BreadcrumbContext(string name, ImageSource icon, IEnumerable children, bool isList, bool isSetting)
        {
            Name = name;
            Icon = icon;
            this.children = children;
            IsResourceList = isList;
            IsSettingsPage = isSetting;
        }

        public bool IsResourceList { get; private set; }
        public bool IsSettingsPage { get; private set; }

        public IEnumerable children { get; private set; }
        public ImageSource Icon { get; private set; }
        public string Name { get; private set; }
    }
}
