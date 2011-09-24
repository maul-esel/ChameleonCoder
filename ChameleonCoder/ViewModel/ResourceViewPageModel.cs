using System.Collections.Generic;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the ResourceViewPage class
    /// </summary>
    internal sealed class ResourceViewPageModel : ViewModelBase
    {
        internal ResourceViewPageModel(Resources.Interfaces.IResource resource)
        {
            resourceInstance = resource;
        }

        public Resources.Interfaces.IResource Resource { get { return resourceInstance; } }

        private readonly Resources.Interfaces.IResource resourceInstance;

        public IDictionary<string, string> MetaData { get { return resourceInstance.GetMetadata(); } }

        public static string MetaDataKey { get { return Res.VP_MetaDataKey; } }

        public static string MetaDataValue { get { return Res.VP_MetaDataValue; } }
    }
}
