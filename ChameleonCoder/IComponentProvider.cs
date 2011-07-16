using System;

namespace ChameleonCoder
{
    interface IComponentProvider
    {
        void Init(Action<Type, Resources.RichContent.ContentMemberTypeInfo> registerContentMember,
            Action<Type, Resources.ResourceTypeInfo> registerResourceType);
    }
}
