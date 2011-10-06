using System;
using System.ComponentModel;
using System.Windows.Input;
using ChameleonCoder.ViewModel.Interaction;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// an abstract base class for ViewModels
    /// </summary>
    internal abstract class ViewModelBase : SecureNotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            Shared.InformationProvider.LanguageChanged += (v) => UpdateAll();
        }

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

        protected string OnUserInput(string topic, string message)
        {
            var handler = UserInput;
            if (handler != null)
            {
                var args = new UserInputEventArgs(topic, message);
                handler(this, args);
                return args.Input;
            }
            return null;
        }

        protected object OnRepresentationNeeded(ViewModelBase model)
        {
            var handler = RepresentationNeeded;
            System.Diagnostics.Debug.Assert(handler != null, "did not set handler");
            if (handler != null)
            {
                var args = new RepresentationEventArgs(model);
                handler(this, args);
                return args.Representation;
            }
            return null;
        }

        #endregion
    }
}
