using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Plugins
{
    public interface IService
    {
        /// <summary>
        /// called when the app is closed, allows plugins to save unfinished work
        /// </summary>
        void Shutdown();

        /// <summary>
        /// the author of the Language Module
        /// </summary>
        string Author { get; }

        /// <summary>
        /// the custom module version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// an about text displayed in an About Box. This may include credits, copyright, ...
        /// </summary>
        string About { get; }

        /// <summary>
        /// called when the app starts and loads all plugins
        /// </summary>
        /// <param name="Host">the instance of the IServiceHost interface the plugin can use to communicate.</param>
        void Initialize(IServiceHost host);

        /// <summary>
        /// called when the user starts the service
        /// </summary>
        void Call();

        /// <summary>
        /// a GUID that uniquely identifies the service. This should always be the same GUID.
        /// </summary>
        Guid Service { get; }

        /// <summary>
        /// the service's display name
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// a brief description of what the service does
        /// </summary>
        string Description { get; }

        /// <summary>
        /// a image representing the service
        /// </summary>
        System.Windows.Media.ImageSource Icon { get; }

        /// <summary>
        /// defines whether the service is busy or not
        /// </summary>
        bool IsBusy { get; }
    }
}
