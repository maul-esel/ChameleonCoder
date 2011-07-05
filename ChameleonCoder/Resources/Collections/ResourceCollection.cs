using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Collections
{
    /// <summary>
    /// represents a flat collection of resources.
    /// </summary>
    public class ResourceCollection : ObservableCollection<IResource>
    {
        /// <summary>
        /// contains all added instances, identified by their GUID
        /// </summary>
        private Dictionary<Guid, IResource> instances = new Dictionary<Guid, IResource>();

        /// <summary>
        /// adds a new instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        internal new void Add(IResource instance)
        {
            try
            {
                this.instances.Add(instance.GUID, instance);
            }
            catch (System.ArgumentException)
            {
                System.Windows.MessageBox.Show("duplicate resource GUID!");
                throw;
            }

            base.Add(instance);
        }

        /// <summary>
        /// removes an instance from the collection
        /// </summary>
        /// <param name="key">the GUID of the instance to remove</param>
        internal void Remove(Guid key)
        {
            IResource instance = this.GetInstance(key);
            this.instances.Remove(key);
            base.Remove(instance);
        }

        /// <summary>
        /// returns an instance added to the Collection
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>the Resource object</returns>
        internal IResource GetInstance(Guid ID)
        {
            IResource instance;
            this.instances.TryGetValue(ID, out instance);
            return instance;
        }
    }
}
