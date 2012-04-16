using System;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    internal static class ResourceManager
    {
        /// <summary>
        /// contains all resources that don't have a direct parent (top-level resources)
        /// </summary>
        private static ResourceCollection Children;

        /// <summary>
        /// contains a list of ALL resources
        /// </summary>
        private static ResourceCollection FlatList;

        /// <summary>
        /// gets the currently loaded resource
        /// </summary>
        internal static IResource ActiveItem
        {
            get;
            private set;
        }

        /// <summary>
        /// sets the collection instances used by this class.
        /// </summary>
        /// <param name="flat">the instance to use as flat list of all resources</param>
        /// <param name="hierarchy">the instance to use as list of top-level resources</param>
        /// <remarks>this is needed to make it possible to create the instances in XAML,
        /// from where they can be referenced in the UI.</remarks>
        internal static void SetCollections(ResourceCollection flat, ResourceCollection hierarchy)
        {
            FlatList = flat;
            Children = hierarchy;
        }

        /// <summary>
        /// adds a resource
        /// 1) to the list of ALL resources
        /// 2) to a given parent list OR the list of top-level resources
        /// It also assigns a method to the resource's 'PropertyChanged' event
        /// </summary>
        /// <param name="instance">the resource to add</param>
        /// <param name="parent">the parent to add the resource to.
        /// If this is null, it will be added to the list of top-level resources</param>
        internal static void Add(IResource instance, IResource parent)
        {
            FlatList.Add(instance);
            if (parent == null)
            {
                Children.Add(instance);
            }
            else
            {
                parent.Children.Add(instance);
            }
            instance.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// removes the resource
        /// 1) from the list of ALL resources
        /// 2) from its parent's Children list OR from the list of top-level resources.
        /// It also removes the handler from the 'PropertyChanged' event.
        /// </summary>
        /// <param name="instance">the instance to remove</param>
        internal static void Remove(IResource instance)
        {
            FlatList.Remove(instance);

            if (instance.Parent == null)
            {
                Children.Remove(instance);
            }
            else
            {
                instance.Parent.Children.Remove(instance);
            }

            instance.PropertyChanged -= OnPropertyChanged;
        }

        /// <summary>
        /// gets the Children list
        /// </summary>
        /// <returns></returns>
        internal static ResourceCollection GetChildren()
        {
            return Children;
        }

        /// <summary>
        /// gets the complete list of all resources
        /// </summary>
        /// <returns></returns>
        internal static ResourceCollection GetList()
        {
            return FlatList;
        }

        public static IResource GetResource(Guid id)
        {
            return FlatList.GetInstance(id);
        }

        /// <summary>
        /// opens a resource
        /// </summary>
        /// <param name="resource">the resource to open</param>
        internal static void Open(IResource resource)
        {
            if (ActiveItem != null)
                Close();

            Shared.InformationProvider.OnResourceLoad(resource, new EventArgs());

            ActiveItem = resource;

            ILanguageResource langRes;

            if ((langRes = resource as ILanguageResource) != null)
            {
                if (PluginManager.ActiveModule != null
                    && langRes.Language != PluginManager.ActiveModule.Identifier)
                {
                    PluginManager.UnloadModule();
                    if (PluginManager.IsModuleRegistered(langRes.Language))
                        PluginManager.LoadModule(langRes.Language);
                }
            }
            Shared.InformationProvider.OnResourceLoaded(resource, new EventArgs());
        }

        /// <summary>
        /// closes the loaded resource
        /// </summary>
        internal static void Close()
        {
            if (ActiveItem != null)
            {
                Shared.InformationProvider.OnResourceUnload(ActiveItem, new EventArgs());

                ILanguageResource langRes = ActiveItem as ILanguageResource;
                if (langRes != null)
                {
                    if (PluginManager.ActiveModule != null && PluginManager.ActiveModule.Identifier == langRes.Language)
                        PluginManager.UnloadModule();
                }

                var item = ActiveItem;
                ActiveItem = null;

                Shared.InformationProvider.OnResourceUnloaded(item, new EventArgs());
            }
            else
                throw new InvalidOperationException("no resource can be closed.");
        }

        /// <summary>
        /// handles changes to a resource and updates the 'last-modified' timestamp
        /// </summary>
        /// <param name="sender">the resource that changed</param>
        /// <param name="args">additional data</param>
        private static void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            IResource resource = sender as IResource;
            resource.UpdateLastModified();
        }
    }
}
