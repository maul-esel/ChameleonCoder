using System;

namespace ChameleonCoder
{
    /// <summary>
    /// an attribute to be applied on an assembly or a class that should be loaded as plugin
    /// </summary>
    /// <remarks>To make your class be loaded as plugin, you must
    /// <list type="1">
    /// <item>implement a plugin interface</item>
    /// <item>make your class public and non-abstract</item>
    /// <item>provide a public, parameterless constructor</item>
    /// <item>apply this attribute on the assembly containing the class</item>
    /// <item>apply this attribute on the class itself</item>
    /// </list>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class), System.Runtime.InteropServices.ComVisible(false)]
    public sealed class CCPluginAttribute : Attribute
    {
    }
}
