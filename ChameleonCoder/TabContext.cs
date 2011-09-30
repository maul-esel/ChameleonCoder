using System;
using System.ComponentModel;
using System.Windows.Controls;
using ChameleonCoder.Interaction;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    internal sealed class TabContext : INotifyPropertyChanged
    {
        internal TabContext(CCTabPage type, Page content)
            : this(type, content, null)
        {
        }

        internal TabContext(CCTabPage type, Page content, object displayed)
        {
            displayedObject = displayed;
            pageType = type;
            contentPage = content;

            InformationProvider.LanguageChanged += OnLanguageChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnLanguageChanged(object value)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("Title"));
            }
        }

        private void OnPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }


        public string Title
        {
            get { return string.Format(titleTemplate, displayedObject); }
        }

        public Page Content
        {
            get { return contentPage; }
            internal set
            {
                contentPage = value;
                OnPropertyChanged("Content");
            }
        }

        internal Object Object
        {
            get { return displayedObject; }
            set
            {
                displayedObject = value;
                OnPropertyChanged("Object");
                OnPropertyChanged("Title");
            }
        }

        internal CCTabPage Type
        {
            get { return pageType; }
            set
            {
                pageType = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("Title");
            }
        }


        private object displayedObject;

        private CCTabPage pageType;

        private Page contentPage;


        private string titleTemplate
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
