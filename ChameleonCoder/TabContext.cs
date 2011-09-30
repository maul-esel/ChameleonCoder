using System;
using System.ComponentModel;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources.Interfaces;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    internal sealed class TabContext : INotifyPropertyChanged
    {
        #region constructors

        internal TabContext(CCTabPage type, object content)
            : this(type, content, null)
        {
        }

        internal TabContext(CCTabPage type, object content, IResource resource)
        {
            Resource = resource;
            Type = type;
            Content = content;

            InformationProvider.LanguageChanged += OnLanguageChanged;
        }

        #endregion

        #region INotifyPropertyChanged

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

        private void OnResourceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                OnPropertyChanged("Title");
        }

        #endregion

        public string Title
        {
            get
            {
                var name = (Resource != null) ? Resource.Name : null;
                return string.Format(TitleTemplate, name);
            }
        }

        public object Content
        {
            get
            {
                return contentObject;
            }

            internal set
            {
                contentObject = value;
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
                if (displayedResource != null)
                    displayedResource.PropertyChanged -= OnResourceChanged;

                displayedResource = value;

                if (displayedResource != null)
                    displayedResource.PropertyChanged += OnResourceChanged;

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

        private object contentObject;


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