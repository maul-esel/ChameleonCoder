using System;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Interaction
{
    public static class InformationProvider
    {
        public static string ProgrammingDirectory { get { return Properties.Settings.Default.ProgrammingDir; } }
        public static int Language { get { return Properties.Settings.Default.Language; } }

        public static IResource GetInstance(Guid id)
        {
            return ResourceManager.GetList().GetInstance(id);
        }

        public static ResourceTypeInfo GetInfo(Type type)
        {
            return new ResourceTypeInfo(ResourceTypeManager.GetInfo(type));
        }
    }
}
