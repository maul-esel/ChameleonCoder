using System;
using System.ComponentModel;
using System.Windows.Input;
using ChameleonCoder.UI.ViewModel.Interaction;

namespace ChameleonCoder.UI.ViewModel
{
    /// <summary>
    /// an abstract base class for ViewModels
    /// </summary>
    internal abstract class ViewModelBase : SecureNotifyPropertyChanged, IAppComponent
    {
        protected ViewModelBase(IChameleonCoderApp app)
        {
            appInstance = app;
            Shared.InformationProvider.LanguageChanged += (v) => UpdateAll();
        }

        public IChameleonCoderApp App
        {
            get { return appInstance; }
        }

        private readonly IChameleonCoderApp appInstance = null;

        #region INotifyPropertyChanged

        internal virtual void UpdateAll()
        {
            foreach (var property in GetType().GetProperties())
            {
                var attr = (NotifyParentPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(NotifyParentPropertyAttribute));
                if (attr == null || attr.NotifyParent == true)
                    OnPropertyChanged(property.Name);
            }
        }

        #endregion

        #region commanding

        /// <summary>
        /// contains the collection of command bindings handled by this model
        /// </summary>
        public CommandBindingCollection Commands
        {
            get { return commandList; }
        }

        private readonly CommandBindingCollection commandList = new CommandBindingCollection();

        #endregion

        #region events

        internal event EventHandler<ReportEventArgs> Report;

        internal event EventHandler<ConfirmationEventArgs> Confirm;

        internal event EventHandler<UserInputEventArgs> UserInput;

        internal event EventHandler<RepresentationEventArgs> RepresentationNeeded;

        protected void OnReport(string topic, string message, MessageSeverity severity)
        {
            var handler = Report;
            if (handler != null)
            {
                handler(this, new ReportEventArgs(topic, message, severity));
            }
        }

        protected bool? OnConfirm(string topic, string message)
        {
            var handler = Confirm;
            if (handler != null)
            {
                var args = new ConfirmationEventArgs(topic, message);
                handler(this, args);
                return args.Accepted;
            }
            return null;
        }

        protected UserInputEventArgs OnUserInput(string topic, string message)
        {
            var handler = UserInput;
            if (handler != null)
            {
                var args = new UserInputEventArgs(topic, message);
                handler(this, args);
                return args;
            }
            return null;
        }

        protected RepresentationEventArgs OnRepresentationNeeded(ViewModelBase model, bool show)
        {
            var handler = RepresentationNeeded;
            if (handler != null)
            {
                var args = new RepresentationEventArgs(model, show);
                handler(this, args);
                return args;
            }
            return null;
        }

        #endregion
    }
}
