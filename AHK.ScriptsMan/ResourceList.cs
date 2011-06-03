using System;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace AHKScriptsMan
{
    /// <summary>
    /// lists all resources
    /// </summary>
    internal sealed class ResourceList
    {
        private static SortedList identifiers = new SortedList();
        private static SortedList instances = new SortedList();

        internal static void Add(int hash, Guid ID, object instance)
        {
            identifiers.Add(hash, ID);
            instances.Add(ID, instance);
        }

        internal static void AddLink(int hash, Guid ID)
        {
            identifiers.Add(hash, ID);
        }

        internal static IResource GetInstance(int hash)
        {
            return GetInstance((Guid)identifiers.GetByIndex(identifiers.IndexOfKey(hash)));
        }

        internal static IResource GetInstance(Guid ID)
        {
            return (IResource)instances.GetByIndex(instances.IndexOfKey(ID));
        }
    }
}
