using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the PluginPage class
    /// </summary>
    internal sealed class PluginPageModel : ViewModelBase
    {
        private PluginPageModel()
        {
            Commands.Add(new CommandBinding(ChameleonCoderCommands.UninstallPlugin,
                UninstallPluginCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.InstallPlugin,
                InstallPluginCommandExecuted));

            plugins.CollectionChanged += (s, e) => OnPropertyChanged("PluginList");

            Shared.InformationProvider.PluginInstalled += (s, e) => plugins.Add(s as IPlugin);
            Shared.InformationProvider.PluginUninstalled += (s, e) => plugins.Remove(s as IPlugin);
        }

        #region singletone

        public static PluginPageModel Instance
        {
            get
            {
                lock (modelInstance)
                {
                    return modelInstance;
                }
            }
        }

        private static readonly PluginPageModel modelInstance = new PluginPageModel();

        #endregion

        #region commanding

        private void UninstallPluginCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UninstallPlugin(e.Parameter as IPlugin);
        }

        private void InstallPluginCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            InstallPlugins();
        }

        #endregion

        public ObservableCollection<IPlugin> PluginList
        {
            get
            {
                return plugins;
            }
        }

        private readonly ObservableCollection<IPlugin> plugins = new ObservableCollection<IPlugin>(PluginManager.GetPlugins());

        #region localization

        public static string Types_All { get { return Res.PP_TypesAll; } }

        public static string Types_Templates { get { return Res.PP_TypesTemplate; } }

        public static string Types_Services { get { return Res.PP_TypesService; } }

        public static string Types_LanguageModules { get { return Res.PP_TypesLanguageModule; } }

        public static string Types_ResourceFactories { get { return Res.PP_TypesResourceFactory; } }

        public static string Uninstall { get { return Res.PP_Uninstall; } }

        public static string Install { get { return Res.PP_Install; } }

        #endregion

        #region (un-)installation

        private void UninstallPlugin(IPlugin plugin)
        {
            Settings.ChameleonCoderSettings.Default.InstalledPlugins.Remove(plugin.Identifier.ToString("n"));
            Shared.InformationProvider.OnPluginUninstalled(plugin);
            Settings.ChameleonCoderSettings.Default.Save();
        }

        private void InstallPlugins()
        {
            var path = OnPluginNeeded(Res.Status_InstallPlugin + " " + Res.Plugin_SelectInstall);

            if (path != null)
            {
                if (!System.IO.File.Exists(path))
                {
                    OnReport(Res.Status_InstallPlugin, Res.Error_InvalidFile, Interaction.MessageSeverity.Error);
                    return;
                }

                Assembly dll;
                try
                {
                    dll = Assembly.LoadFile(path);
                }
                catch (BadImageFormatException)
                {
                    OnReport(Res.Status_InstallPlugin, string.Format(Res.Error_InstallNoAssembly, path), Interaction.MessageSeverity.Error);
                    return;
                }

                if (!Attribute.IsDefined(dll, typeof(CCPluginAttribute)))
                {
                    OnReport(Res.Status_InstallPlugin, string.Format(Res.Error_InstallNoPlugin, path), Interaction.MessageSeverity.Error);
                    App.Log(GetType().ToString() + " --> private void Install(object, EventArgs)",
                        "refused plugin install: Assembly does not have CCPluginAttribute defined (" + path + ").",
                        null);
                    return;
                }

                var newPlugins = new ObservableCollection<IPlugin>();

                foreach (var type in dll.GetTypes())
                {
                    if (type.IsDefined(typeof(CCPluginAttribute), false)
                        && !type.IsValueType && !type.IsAbstract && type.IsClass && type.IsPublic
                        && type.GetInterface(typeof(IPlugin).FullName) != null
                        && type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                        if (!Settings.ChameleonCoderSettings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
                            newPlugins.Add(plugin);
                    }
                }

                if (newPlugins.Count == 0)
                {
                    OnReport(Res.Status_InstallPlugin, string.Format(Res.Error_InstallEmptyAssembly, path), Interaction.MessageSeverity.Critical);
                    App.Log(GetType().ToString() + " --> private void Install(object, EventArgs)",
                        "refused plugin install: Assembly does not contain plugin classes that aren't already installed (" + path + ").",
                        null);
                    return;
                }

                var representation = OnRepresentationNeeded(new PluginInstallerModel(newPlugins), true);
                Settings.ChameleonCoderSettings.Default.Save();
            }
        }

        #endregion

        #region events

        /// <summary>
        /// called when the model needs to know the path to a plugin assembly
        /// </summary>
        internal event EventHandler<Interaction.FileSelectionEventArgs> PluginNeeded;

        private string OnPluginNeeded(string message)
        {
            var handler = PluginNeeded;

            if (handler != null)
            {
                var args = new Interaction.FileSelectionEventArgs(message, System.IO.Path.Combine(App.AppDir, "Components"), "CC plugins | *.dll", true);
                handler(this, args);
                return args.Path;
            }
            return null;
        }

        #endregion
    }
}
