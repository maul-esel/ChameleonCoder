﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying resource details
    /// </summary>
    public partial class ResourceViewPage : Page
    {
        /// <summary>
        /// creates a new instance of this page, given a resource to display
        /// </summary>
        /// <param name="resource">the resource to display</param>
        internal ResourceViewPage(IResource resource)
        {
            Resource = ResourceManager.ActiveItem = resource;
            DataContext = new { res = resource, meta = resource.GetMetadata(), lang = App.Gui.DataContext };

            InitializeComponent();

            ILanguageResource langRes = resource as ILanguageResource;
            if (langRes != null)
            {
                if (PluginManager.ActiveModule != null)
                    PluginManager.UnloadModule();
                if (PluginManager.IsModuleRegistered(langRes.language))
                    PluginManager.LoadModule(langRes.language);
            }
        }

        internal void AddMetadata(string name)
        {
            Resource.SetMetadata(name, null);
            DataContext = new { res = Resource, meta = Resource.GetMetadata(), lang = App.Gui.DataContext };
        }

        internal void DeleteMetadata()
        {
            if (MetadataGrid.SelectedIndex != -1)
            {
                Resource.DeleteMetadata(((KeyValuePair<string, string>)MetadataGrid.SelectedItem).Key);
                DataContext = new { res = Resource, meta = Resource.GetMetadata(), lang = App.Gui.DataContext };
            }
        }

        private void SaveMetadata(object sender, EventArgs e)
        {
            string value = (sender as TextBox).Text;
            string key = ((KeyValuePair<string, string>)(((sender as TextBox).TemplatedParent as ContentPresenter).Parent as GridViewRowPresenter).Content).Key;

            Resource.SetMetadata(key, value);
            (sender as TextBox).InvalidateProperty(System.Windows.FrameworkElement.WidthProperty);
        }

        /// <summary>
        /// the resource which is displayed
        /// </summary>
        internal IResource Resource { get; private set; }
    }
}
