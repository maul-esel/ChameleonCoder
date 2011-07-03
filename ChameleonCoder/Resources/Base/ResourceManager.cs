using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Base
{
    internal class ResourceManager
    {
        /// <summary>
        /// contains all resources that don't have a direct parent (top-level resources)
        /// </summary>
        internal static ResourceCollection children;

        /// <summary>
        /// contains a list of ALL resources
        /// </summary>
        internal static ResourceCollection FlatList;

        internal static IResource ActiveItem;


        /// <summary>
        /// adds a resource
        /// 1) to the list of ALL resources
        /// 2) to a given parent list OR the list of top-level resources
        /// </summary>
        /// <param name="instance">the resource to add</param>
        /// <param name="parentlist">the parent list to add the resource to.
        /// If this is null, it will be added to the list of top-level resources</param>
        internal static void Add(ResourceBase instance, ResourceCollection parentlist)
        {
            FlatList.Add(instance);

            if (parentlist == null)
            {
                children.Add(instance);
            }
            else
            {
                parentlist.Add(instance);
            }
        }

        // needed: remove methods
    }
}
