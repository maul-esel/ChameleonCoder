using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ChameleonCoder.Services;

namespace GuidCreator
{
    public class GuidCreator : IService
    {
        #region infrastructure

        IServiceHost Host;

        bool _busy;

        System.Windows.Media.ImageSource _icon;

        System.Windows.Forms.Form _presenter;

        #endregion

        #region IService

        void IService.Shutdown()
        {
        }

        string IService.Author { get { return "maul.esel"; } }
        string IService.Version { get { return "1.0"; } }
        string IService.About { get { return "small example service to create a \n Globally Unique IDentifier.\n Coded by maul.esel 2011"; } }
        Guid IService.Service { get { return new Guid("{fa55bce0-5341-4007-83c6-e5e985bd3f22}"); } }
        string IService.ServiceName { get { return "GuidCreator"; } }
        string IService.Description { get { return "creates a GUID"; } }
        bool IService.IsBusy { get { return _busy; } }
        System.Windows.Media.ImageSource IService.Icon { get { return _icon; } }

        void IService.Initialize(IServiceHost host)
        {
            this.Host = host;

            this._icon = Imaging.CreateBitmapSourceFromHBitmap(Icon.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            System.Windows.Forms.TextBox outbox = new System.Windows.Forms.TextBox();
            outbox.Width = 250; outbox.Margin = new System.Windows.Forms.Padding(10);
            outbox.Name = "OutputBox";

            _presenter = new System.Windows.Forms.Form();
            _presenter.Controls.Add(outbox);
        }

        void IService.Call()
        {
            _presenter.Controls[_presenter.Controls.IndexOfKey("OutputBox")].Text = Guid.NewGuid().ToString("b");
            _presenter.ShowDialog();
        }

        #endregion
    }
}
