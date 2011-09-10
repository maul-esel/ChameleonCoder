using System;

namespace ChameleonCoder
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public sealed class CCPluginAttribute : Attribute
    {
    }
}
