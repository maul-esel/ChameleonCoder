using System;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    internal sealed class ResourceTypeCollection : ComponentCollection<string, Type>
    {
        internal void RegisterResourceType(Type resourceType)
        {
            base.RegisterComponent((Activator.CreateInstance(resourceType) as ResourceBase).Alias, resourceType);
        }

        internal Type GetResourceType(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
