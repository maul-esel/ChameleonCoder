namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a base interface to be implemented by components
    /// </summary>
    public interface IPlugin : IComponent
    {
        /// <summary>
        /// an about text displayed in an About Box. This may include credits, copyright, ...
        /// </summary>
        string About { get; }

        /// <summary>
        /// the author of the component
        /// </summary>
        string Author { get; }

        /// <summary>
        /// a brief description of what the service does
        /// </summary>
        string Description { get; }

        /// <summary>
        /// the component version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// called when the app starts and loads all plugins
        /// </summary>
        void Initialize(ChameleonCoderApp app);

        /// <summary>
        /// called when the app is closed, allows plugins to save unfinished work
        /// </summary>
        void Shutdown();

        /// <summary>
        /// a backreference to the app the plugin is loaded by
        /// </summary>
        ChameleonCoderApp App { get; }
    }
}
