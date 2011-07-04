using System;

namespace ChameleonCoder.Resources.Base
{
    /// <summary>
    /// defines the resource's type
    /// </summary>
    [Obsolete("use Type instead to be more flexible")]
    public enum ResourceType
    {
        /// <summary>
        /// abstract base type for resources
        /// </summary>
        resource,

        /// <summary>
        /// the element represents a link to a resource
        /// </summary>
        link,

        /// <summary>
        /// the resource is a file
        /// </summary>
        file,

        /// <summary>
        /// a file resource specified on code files
        /// </summary>
        code,

        /// <summary>
        /// the resource is a library
        /// </summary>
        library,

        /// <summary>
        /// the resource is a project
        /// </summary>
        project,

        /// <summary>
        /// the resource is a task
        /// </summary>
        task
    }
}
