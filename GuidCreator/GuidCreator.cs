using System;
using System.Windows.Media.Imaging;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Services
{
    /// <summary>
    /// an example service, creating new GUIDs
    /// </summary>
    [ChameleonCoder.CCPlugin]
    public sealed class GuidCreator : IService
    {
        #region infrastructure

        bool busy;

        #endregion

        #region IService

        /// <summary>
        /// shuts down the service
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// gets the service author(s)
        /// </summary>
        public string Author
        {
            get { return "maul.esel"; }
        }

        /// <summary>
        /// gets the service version
        /// </summary>
        public string Version
        {
            get { return "1.0"; }
        }

        /// <summary>
        /// gets the 'About'-information for the service
        /// </summary>
        public string About
        {
            get { return "© by maul.esel 2011"; }
        }

        /// <summary>
        /// gets the unique identifier for the service
        /// </summary>
        public Guid Identifier
        {
            get { return new Guid("{fa55bce0-5341-4007-83c6-e5e985bd3f22}"); }
        }

        /// <summary>
        /// gets the service's name
        /// </summary>
        public string Name
        {
            get { return "GuidCreator"; }
        }

        /// <summary>
        /// gets the service's description
        /// </summary>
        public string Description
        {
            get { return "small example service to create Globally Unique IDentifiers."; }
        }

        /// <summary>
        /// gets whether the service is currently busy or not
        /// </summary>
        public bool IsBusy
        {
            get { return busy; }
        }

        /// <summary>
        /// gets the service's icon
        /// </summary>
        public System.Windows.Media.ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/GuidCreator;component/icon.png")).GetAsFrozen() as BitmapImage; }
        }

        /// <summary>
        /// initializes the service
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// executes the service
        /// </summary>
        public void Execute()
        {
            Properties.Resources.Culture = new System.Globalization.CultureInfo(Interaction.InformationProvider.Language);

            busy = true;
            CreatorView viewer = new CreatorView();
            viewer.ShowDialog();
            busy = false;
        }

        #endregion
    }
}
