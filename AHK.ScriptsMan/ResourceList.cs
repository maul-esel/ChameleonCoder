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
    public class ResourceList
    {
        public static SortedList identifiers = new SortedList();
        public static SortedList instances = new SortedList();
        public static SortedList types = new SortedList();

        public static void Add(int hash, Guid ID, ResourceType type, object instance)
        {
            identifiers.Add(hash, ID);
            instances.Add(ID, instance);
            types.Add(ID, type);
        }

        public static T GetInstance<T>(IntPtr ID)
        {
            return GetInstance<T>((Guid)identifiers.GetByIndex(identifiers.IndexOfKey(ID)));
        }

        public static T GetInstance<T>(Guid ID)
        {
            return (T)instances.GetByIndex(instances.IndexOfKey(ID));
        }

        public static ResourceType GetType(IntPtr ID)
        {
            return GetType((Guid)identifiers.GetByIndex(identifiers.IndexOfKey(ID)));
        }

        public static ResourceType GetType(Guid ID)
        {
            return (ResourceType)types.GetByIndex(types.IndexOfKey(ID));
        }
        
    }
}
