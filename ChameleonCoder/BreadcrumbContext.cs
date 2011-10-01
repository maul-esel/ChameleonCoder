using System;
using System.Collections;
using System.Windows.Media;
using ChameleonCoder.Interaction;

namespace ChameleonCoder
{
    internal sealed class BreadcrumbContext
    {
        internal BreadcrumbContext(string name, ImageSource icon, IEnumerable children, CCTabPage pageType)
        {
            switch (pageType)
            {
                case CCTabPage.ResourceEdit:
                case CCTabPage.ResourceView:
                case CCTabPage.None:
                    throw new InvalidOperationException("specified page type is not valid.");

                default:
                    PageType = pageType;
                    break;
            }
            Name = name;
            Icon = icon;
            Children = children;
        }

        internal CCTabPage PageType { get; private set; }

        public IEnumerable Children { get; private set; }

        public ImageSource Icon { get; private set; }

        public string Name { get; private set; }
    }    
}
