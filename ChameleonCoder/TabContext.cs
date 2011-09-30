using System;
using System.ComponentModel;
using System.Windows.Controls;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources.Interfaces;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    internal sealed class TabContext : INotifyPropertyChanged
    {
        internal TabContext(CCTabPage type, Page content)
            : this(type, content, null)
        {
        }

        internal TabContext(CCTabPage type, Page content, IResource resource)
        {
            Resource = resource;
            Type = type;
            Content = content;

            InformationProvider.LanguageChanged += OnLanguageChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }


        private void OnLanguageChanged(object value)
        {
            OnPropertyChanged("Title");
        }


        public string Title
        {
            get
            {
                var name = (Resource != null) ? Resource.Name : null;
                return string.Format(TitleTemplate, name);
            }
        }

        public Page Content
        {
            get
            {
                return contentPage;
            }

            internal set
            {
                contentPage = value;
                OnPropertyChanged("Content");
            }
        }

        internal IResource Resource
        {
            get
            {
                return displayedResource;
            }

            set
            {
                displayedResource = value;
                OnPropertyChanged("Resource");
                OnPropertyChanged("Title");
            }
        }

        internal CCTabPage Type
        {
            get
            {
                return pageType;
            }

            set
            {
                pageType = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("Title");
            }
        }


        private IResource displayedResource;

        private CCTabPage pageType;

        private Page contentPage;


        private string TitleTemplate
        {
            get
            {
                switch (pageType)
                {
                    case CCTabPage.Home:
                        return Res.Item_Home;

                    case CCTabPage.Plugins:
                        return Res.Item_Plugins;

                    case CCTabPage.ResourceEdit:
                        return Res.Item_ResourceEdit;

                    case CCTabPage.ResourceList:
                        return Res.Item_List;

                    case CCTabPage.ResourceView:
                        return Res.Item_ResourceView;

                    case CCTabPage.Settings:
                        return Res.Item_Settings;

                    default:
                    case CCTabPage.None:
                        throw new InvalidOperationException("page type is not valid");
                }
            }
        }
    }
}
