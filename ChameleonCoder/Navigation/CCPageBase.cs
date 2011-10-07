using System.Windows;

namespace ChameleonCoder.Navigation
{
    internal abstract class CCPageBase : System.Windows.Controls.Page
    {
        protected void Initialize(ViewModel.ViewModelBase model)
        {
            model.Report -= ReportMessage; // remove handler in case already listening
            model.Report += ReportMessage; // add it (again)

            model.Confirm -= ConfirmMessage;
            model.Confirm += ConfirmMessage;

            model.UserInput -= GetInput;
            model.UserInput += GetInput;

            DataContext = model;
            CommandBindings.AddRange(model.Commands);
        }

        private static void ReportMessage(object sender, ViewModel.Interaction.ReportEventArgs e)
        {
            MessageBoxImage icon;
            switch (e.Severity)
            {
                case ViewModel.Interaction.MessageSeverity.Error:
                    icon = MessageBoxImage.Error;
                    break;

                case ViewModel.Interaction.MessageSeverity.Critical:
                    icon = MessageBoxImage.Exclamation;
                    break;

                default:
                case ViewModel.Interaction.MessageSeverity.Information:
                    icon = MessageBoxImage.Information;
                    break;
            }

            MessageBox.Show(e.Message, e.Topic, MessageBoxButton.OK, icon);
        }

        private static void ConfirmMessage(object sender, ViewModel.Interaction.ConfirmationEventArgs e)
        {
            e.Accepted = MessageBox.Show(e.Message,
                                        e.Topic,
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private static void GetInput(object sender, ViewModel.Interaction.UserInputEventArgs e)
        {
            var box = new Shared.InputBox(e.Topic, e.Message);
            if (box.ShowDialog() == true)
                e.Input = box.Text;
        }
    }
}
