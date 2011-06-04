using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ChameleonCoder
{
    internal class TreeNodeSorter : IComparer
    {
        int IComparer.Compare(object obj1, object obj2)
        {
            IResource resource1 = ResourceList.GetInstance(obj1.GetHashCode());
            IResource resource2 = ResourceList.GetInstance(obj2.GetHashCode());

            if (resource1.Type < resource2.Type)
            {
                return -1;
            }
            else if (resource1.Type > resource2.Type)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
