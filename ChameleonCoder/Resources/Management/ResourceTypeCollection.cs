using System;

namespace ChameleonCoder.Resources.Management
{
    internal sealed class ResourceTypeCollection : ComponentCollection<Guid, Type>
    {
        internal void RegisterResourceType(Guid key, Type resourceType)
        {
            base.RegisterComponent(key, resourceType);
        }

        internal Type GetResourceType(Guid key)
        {
            return base.GetComponent(key);
        }

        internal Guid GetAlias(Type type)
        {
            return base.GetKey(type);
        }
    }
}
