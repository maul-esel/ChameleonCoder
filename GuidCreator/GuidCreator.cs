using System;
using System.Windows.Media.Imaging;
using ChameleonCoder.Services;

namespace GuidCreator
{
    public class GuidCreator : IService
    {
        #region infrastructure

        IServiceHost host;

        bool busy;

        System.Windows.Media.ImageSource icon;

        #endregion

        #region IService

        public void Shutdown()
        {
        }

        public string Author { get { return "maul.esel"; } }
        public string Version { get { return "1.0"; } }
        public string About { get { return "small example service to create a \n Globally Unique IDentifier.\n Coded by maul.esel 2011"; } }
        public Guid Service { get { return new Guid("{fa55bce0-5341-4007-83c6-e5e985bd3f22}"); } }
        public string ServiceName { get { return "GuidCreator"; } }
        public string Description { get { return "creates a GUID"; } }
        public bool IsBusy { get { return busy; } }
        public System.Windows.Media.ImageSource Icon { get { return icon; } }

        public void Initialize(IServiceHost host)
        {
            this.host = host;

            this.icon = new BitmapImage(new Uri("pack://application:,,,/GuidCreator;component/icon.png"));
        }

        public void Call()
        {
            busy = true;
            CreatorView viewer = new CreatorView();
            busy = false;
        }

        #endregion
    }
}
