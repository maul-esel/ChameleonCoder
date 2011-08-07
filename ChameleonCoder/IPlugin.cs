namespace ChameleonCoder
{
    /// <summary>
    /// a base interface to be implemented by components
    /// </summary>
    public interface IPlugin
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
        /// a image representing the  component
        /// </summary>
        System.Windows.Media.ImageSource Icon { get; }

        /// <summary>
        /// a GUID that uniquely identifies the component. This should always be the same GUID.
        /// </summary>
        System.Guid Identifier { get; }

        /// <summary>
        /// the component's display name
        /// </summary>
        string Name { get; }

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
