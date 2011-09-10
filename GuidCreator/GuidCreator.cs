using System;
using System.Windows.Media.Imaging;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Services
{
    [ChameleonCoder.CCPlugin]
    public class GuidCreator : IService
    {
        #region infrastructure

        bool busy;

        #endregion

        #region IService

        public void Shutdown()
        {
        }

        public string Author { get { return "maul.esel"; } }
        public string Version { get { return "1.0"; } }
        public string About { get { return "© by maul.esel 2011"; } }
        public Guid Identifier { get { return new Guid("{fa55bce0-5341-4007-83c6-e5e985bd3f22}"); } }
        public string Name { get { return "GuidCreator"; } }
        public string Description { get { return "small example service to create a Globally Unique IDentifier."; } }
        public bool IsBusy { get { return busy; } }
        public System.Windows.Media.ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/GuidCreator;component/icon.png")).GetAsFrozen() as BitmapImage; } }

        public void Initialize()
        {
        }

        public void Execute()
        {
            busy = true;
            CreatorView viewer = new CreatorView();
            busy = false;
        }

        #endregion
    }
}
