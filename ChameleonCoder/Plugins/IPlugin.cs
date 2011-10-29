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
        /// the component version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// called when the app starts and loads all plugins
        /// </summary>
        void Initialize();

        /// <summary>
        /// called when the app is closed, allows plugins to save unfinished work
        /// </summary>
        void Shutdown();
    }
}
