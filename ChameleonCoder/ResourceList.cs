using System;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace ChameleonCoder
{
    /// <summary>
    /// lists all resources
    /// </summary>
    internal sealed class ResourceList
    {
        private static SortedList identifiers = new SortedList();
        private static SortedList instances = new SortedList();
        private static Guid Active = Guid.Empty;

        internal static void Add(int hash, Guid ID, object instance)
        {
            identifiers.Add(hash, ID);
            instances.Add(ID, instance);
        }

        internal static void AddLink(int hash, Guid ID)
        {
            identifiers.Add(hash, ID);
        }

        internal static void Remove(int hash)
        {
            Guid key = (Guid)identifiers.GetByIndex(identifiers.IndexOfKey(hash));
            identifiers.Remove(hash);
            instances.Remove(key);
        }

        internal static void RemoveLink(int hash)
        {
            identifiers.Remove(hash);
        }

        internal static cResource GetInstance(int hash)
        {
            return GetInstance((Guid)identifiers.GetByIndex(identifiers.IndexOfKey(hash)));
        }

        internal static cResource GetInstance(Guid ID)
        {
            return (cResource)instances.GetByIndex(instances.IndexOfKey(ID));
        }

        internal static cResource GetActiveInstance()
        {
            if (Active != Guid.Empty)
            {
                return GetInstance(Active);
            }
            return null;
        }

        internal static void SetActiveInstance(Guid ID)
        {
            Active = ID;
        }

        internal static void SetActiveInstance(int hash)
        {
            SetActiveInstance((Guid)identifiers.GetByIndex(identifiers.IndexOfKey(hash)));
        }
    }
}
