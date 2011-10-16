using System;
using System.Windows;
using System.Reflection;
using ChameleonCoder.ViewModel.Interaction;

namespace ChameleonCoder
{
    /// <summary>
    /// a helper class for model clients to avoid duplicate code
    /// </summary>
    internal static class ModelClientHelper
    {
        /// <summary>
        /// gets a view for a given view model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void GetModelRepresentation(object sender, RepresentationEventArgs e)
        {
            if (e.Model != null)
            {
                var attr = (DefaultRepresentationAttribute)
                    Attribute.GetCustomAttribute(e.Model.GetType(), typeof(DefaultRepresentationAttribute));

                if (attr != null) // if the attribute is defined
                {
                    Type representationType = attr.RepresentationType;
                    if (representationType != null) // if a valid type is given
                    {
                        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                        if (representationType.GetConstructor(flags, null, Type.EmptyTypes, null) != null) // if a parameterless constructor is defined
                        {
                            e.Representation = Activator.CreateInstance(representationType, flags, null, null, null); // create the instance
                        }
                        else
                        {
                            e.Representation = Activator.CreateInstance(representationType, flags, null, new object[1] { e.Model }, null); // assume a constructor with 1 param (model) is defined
                        }
                    }
                }

                if (e.ShowRepresentation && e.Representation != null)
                {
                    var dialog = e.Representation as Window;
                    if (dialog != null)
                    {
                        if (dialog.ShowDialog() != true)
                            e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// reports a message by the view model to the user
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void ReportMessage(object sender, ReportEventArgs e)
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

        /// <summary>
        /// confirms a message by the view model by letting the user decide
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void ConfirmMessage(object sender, ConfirmationEventArgs e)
        {
            e.Accepted = MessageBox.Show(e.Message,
                                        e.Topic,
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// gets user input for the view model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void GetInput(object sender, UserInputEventArgs e)
        {
            var box = new Shared.InputBox(e.Topic, e.Message);
            if (box.ShowDialog() == true)
            {
                e.Input = box.Text;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// lets the user select a file for the model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void SelectFile(object sender, FileSelectionEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = e.Filter,
                                                                            Title = e.Message,
                                                                            CheckFileExists = e.MustExist,
                                                                            InitialDirectory = e.Directory })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    e.Path = dialog.FileName;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// lets the user select a directory
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        internal static void SelectDirectory(object sender, DirectorySelectionEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog() { Description = e.Message,
                                                                                 SelectedPath = e.InitialDirectory,
                                                                                 ShowNewFolderButton = e.AllowCreation })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    e.Path = dialog.SelectedPath;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// initializes a model with several event handlers
        /// </summary>
        /// <param name="model"></param>
        internal static void InitializeModel(ViewModel.ViewModelBase model)
        {
            model.Report -= ModelClientHelper.ReportMessage; // remove handler in case already listening
            model.Report += ModelClientHelper.ReportMessage; // add it (again)

            model.Confirm -= ModelClientHelper.ConfirmMessage;
            model.Confirm += ModelClientHelper.ConfirmMessage;

            model.UserInput -= ModelClientHelper.GetInput;
            model.UserInput += ModelClientHelper.GetInput;

            model.RepresentationNeeded -= ModelClientHelper.GetModelRepresentation;
            model.RepresentationNeeded += ModelClientHelper.GetModelRepresentation;
        }
    }
}
