using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the ResourceViewPage class
    /// </summary>
    internal sealed class ResourceViewPageModel : ViewModelBase
    {
        internal ResourceViewPageModel(IResource resource)
        {
            resourceInstance = resource;
        }

        public IResource Resource { get { return resourceInstance; } }

        private readonly IResource resourceInstance;

        public IDictionary<string, string> Metadata { get { return resourceInstance.GetMetadata(); } }

        public static string MetadataKey { get { return Res.VP_MetadataKey; } }

        public static string MetadataValue { get { return Res.VP_MetadataValue; } }
    }
}
