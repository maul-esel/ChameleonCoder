using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChameleonCoder.Resources.Collections
{
    [Obsolete("binding should make this unnecessary, using the new ResourceCollection class", false)]
    class LinkCollection : ObservableCollection<Guid>
    {
        private Dictionary<int, Guid> identifiers = new Dictionary<int, Guid>();

        public void Add(int hash, Guid ID)
        {
            this.identifiers.Add(hash, ID);
            base.Add(ID);
        }

        public void Remove(int hash)
        {
            Guid ID = this.GetGUID(hash);
            this.identifiers.Remove(hash);
            base.Remove(ID);
        }

        public Guid GetGUID(int hash)
        {
            Guid link;
            this.identifiers.TryGetValue(hash, out link);
            return link;
        }
    }
}
