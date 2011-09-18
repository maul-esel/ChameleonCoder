namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// represents the type of the currently opened tab page
    /// </summary>
    public enum CCTabPage
    {
        /// <summary>
        /// no page
        /// </summary>
        None,

        /// <summary>
        /// the "welcome" page
        /// </summary>
        Home,

        /// <summary>
        /// the configuration page
        /// </summary>
        Settings,

        /// <summary>
        /// the plugin page
        /// </summary>
        Plugins,

        /// <summary>
        /// the page with a list of all resources
        /// </summary>
        ResourceList,

        /// <summary>
        /// a page showing a resource's properties
        /// </summary>
        ResourceView,
        
        /// <summary>
        /// a page showing a resource's editable contents
        /// </summary>
        ResourceEdit
    }
}
