using System;

namespace ChameleonCoder
{
    interface IComponentProvider
    {
        void Init(Action<Type, string> registerContentMember, Action<Type, Resources.ResourceTypeInfo> registerResourceType);
    }
}
