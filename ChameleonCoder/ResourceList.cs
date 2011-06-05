﻿using System;
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