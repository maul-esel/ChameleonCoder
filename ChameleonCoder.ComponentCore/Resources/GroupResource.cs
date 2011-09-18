using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a simple group of resources
    /// </summary>
    public class GroupResource : ResourceBase
    {
        /// <summary>
        /// gets the icon that represents this instance to the user
        /// </summary>
        /// <value>This is always the same as the GroupResource's type icon.</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/group.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        internal const string Alias = "group";
    }
}
