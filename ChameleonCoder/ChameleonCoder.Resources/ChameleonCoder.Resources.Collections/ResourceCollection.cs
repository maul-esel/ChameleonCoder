using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Collections
{
    /// <summary>
    /// represents a flat collection of resources.
    /// </summary>
    public class ResourceCollection : ObservableCollection<ResourceBase>
    {
        /// <summary>
        /// contains all added instances, identified by their GUID
        /// </summary>
        private Dictionary<Guid, ResourceBase> instances = new Dictionary<Guid, ResourceBase>();

        /// <summary>
        /// adds a new instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        public new void Add(ResourceBase instance)
        {
            try
            {
                this.instances.Add(instance.GUID, instance);
            }
            catch (System.ArgumentException e)
            {
                System.Windows.MessageBox.Show("duplicate resource GUID!");
                throw e;
            }

            base.Add(instance);
        }

        /// <summary>
        /// removes an instance from the collection
        /// </summary>
        /// <param name="key">the GUID of the instance to remove</param>
        internal void Remove(Guid key)
        {
            ResourceBase instance = this.GetInstance(key);
            this.instances.Remove(key);
            base.Remove(instance);
        }

        /// <summary>
        /// returns an instance added to the Collection
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>the Resource object</returns>
        internal ResourceBase GetInstance(Guid ID)
        {
            ResourceBase instance;
            this.instances.TryGetValue(ID, out instance);
            return instance;
        }
    }
}
