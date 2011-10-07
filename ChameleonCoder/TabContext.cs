﻿using System;
using System.ComponentModel;
using ChameleonCoder.Shared;
using ChameleonCoder.Resources.Interfaces;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    internal sealed class TabContext : SecureNotifyPropertyChanged
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

                switch (pageType)
                {
                    case CCTabPage.Home:
                        return Res.Item_Home;

                    case CCTabPage.Plugins:
                        return Res.Item_Plugins;

                    case CCTabPage.ResourceEdit:
                        return string.Format(Res.Item_ResourceEdit, name);

                    case CCTabPage.ResourceList:
                        return Res.Item_List;

                    case CCTabPage.ResourceView:
                        return string.Format(Res.Item_ResourceView, name);

                    case CCTabPage.Settings:
                        return Res.Item_Settings;

                    case CCTabPage.FileManagement:
                        return Res.Item_FileManagement; // TODO: handle file name

                    default:
                    case CCTabPage.None:
                        throw new InvalidOperationException("page type is not valid");
                }
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
    }
}