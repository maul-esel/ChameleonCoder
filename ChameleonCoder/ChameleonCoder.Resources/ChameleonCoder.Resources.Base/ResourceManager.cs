using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Base
{
    class ResourceManager
    {
        /// <summary>
        /// contains all resources that don't have a direct parent (top-level resources)
        /// </summary>
        public static ResourceCollection children;

        /// <summary>
        /// contains a list of ALL resources
        /// </summary>
        public static ResourceCollection FlatList = new ResourceCollection();

        /// <summary>
        /// adds a resource
        /// 1) to the list of ALL resources
        /// 2) to a given parent list OR the list of top-level resources
        /// </summary>
        /// <param name="instance">the resource to add</param>
        /// <param name="parentlist">the parent list to add the resource to.
        /// If this is null, it will be added to the list of top-level resources</param>
        public static void Add(ResourceBase instance, ResourceCollection parentlist)
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
