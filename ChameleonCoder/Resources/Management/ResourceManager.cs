using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    internal static class ResourceManager
    {
        /// <summary>
        /// contains all resources that don't have a direct parent (top-level resources)
        /// </summary>
        private static ResourceCollection children;

        /// <summary>
        /// contains a list of ALL resources
        /// </summary>
        private static ResourceCollection FlatList;

        internal static IResource ActiveItem;

        internal static void SetCollections(ResourceCollection flat, ResourceCollection hierarchy)
        {
            FlatList = flat;
            children = hierarchy;
        }

        /// <summary>
        /// adds a resource
        /// 1) to the list of ALL resources
        /// 2) to a given parent list OR the list of top-level resources
        /// </summary>
        /// <param name="instance">the resource to add</param>
        /// <param name="parentlist">the parent list to add the resource to.
        /// If this is null, it will be added to the list of top-level resources</param>
        internal static void Add(IResource instance, IResource parent)
        {
            FlatList.Add(instance);

            if (parent == null)
            {
                children.Add(instance);
            }
            else
            {
                parent.children.Add(instance);
            }
        }

        internal static ResourceCollection GetChildren()
        {
            return children;
        }

        internal static ResourceCollection GetList()
        {
            return FlatList;
        }

        internal static void Open(IResource resource)
        {
            ActiveItem = resource;

            ILanguageResource langRes;

            if ((langRes = resource as ILanguageResource) != null)
            {
                if (LanguageModules.LanguageModuleHost.GetActiveModule() != null
                    && langRes.language != LanguageModules.LanguageModuleHost.GetActiveModule().Language)
                {
                    LanguageModules.LanguageModuleHost.UnloadModule();
                    LanguageModules.LanguageModuleHost.LoadModule(langRes.language);
                }
            }
        }

        // needed: remove methods
    }
}
