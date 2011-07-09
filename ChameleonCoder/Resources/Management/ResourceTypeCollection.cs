using System;

namespace ChameleonCoder.Resources.Management
{
    internal sealed class ResourceTypeCollection : ComponentCollection<string, Type>
    {
        internal void RegisterResourceType(string key, Type resourceType)
        {
            base.RegisterComponent(key, resourceType);
        }

        internal Type GetResourceType(string alias)
        {
            return base.GetComponent(alias);
        }
    }
}
