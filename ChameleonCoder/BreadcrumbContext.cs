using System;
using System.Collections;
using ChameleonCoder.Shared;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class BreadcrumbContext : SecureNotifyPropertyChanged
    {
        internal BreadcrumbContext(Uri icon, IEnumerable children, CCTabPage pageType)
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
            Icon = icon;
            Children = children;

            Shared.InformationProvider.LanguageChanged += v => OnPropertyChanged("Name");
        }

        internal CCTabPage PageType { get; private set; }

        public IEnumerable Children { get; private set; }

        public Uri Icon { get; private set; }

        public string Name
        {
            get
            {
                switch (PageType)
                {
                    case CCTabPage.Home:
                        return Res.Item_Home;

                    case CCTabPage.ResourceList:
                        return Res.Item_List;

                    case CCTabPage.Plugins:
                        return Res.Item_Plugins;

                    case CCTabPage.Settings:
                        return Res.Item_Settings;

                    case CCTabPage.FileManagement:
                        return Res.Item_FileManagement;

                    default:
                        throw new InvalidOperationException("specified page type is not valid.");
                }
            }
        }
    }    
}
