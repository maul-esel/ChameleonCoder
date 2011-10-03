using System;
using System.ComponentModel;
using System.Windows.Input;
using ChameleonCoder.ViewModel.Interaction;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// an abstract base class for ViewModels
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            Shared.InformationProvider.LanguageChanged += (v) => UpdateAll();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        internal virtual void UpdateAll()
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                foreach (var property in GetType().GetProperties())
                {
                    var attr = (NotifyParentPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(NotifyParentPropertyAttribute));
                    if (attr == null || attr.NotifyParent == true)
                        handler(this, new PropertyChangedEventArgs(property.Name));
                }
            }
        }

        internal virtual void Update(string property)
        {
            if (GetType().GetProperty(property) == null)
                throw new InvalidOperationException("update unknown property: " + property);

            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
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

        #endregion
    }
}
