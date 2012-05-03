using System;
using System.Collections.Generic;
using System.IO;

namespace ChameleonCoder.Files
{
    /// <summary>
    /// a class to manage the opened files
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public sealed class FileManager : IFileManager
    {
        /// <summary>
        /// creates a new instance of the class
        /// </summary>
        /// <param name="app">the app instrance the manager belongs to</param>
        internal FileManager(IChameleonCoderApp app)
        {
            App = app;
        }

        /// <summary>
        /// opens a given file
        /// </summary>
        /// <param name="path">the path to the file to open</param>
        /// <returns>the opened file as <see cref="IDataFile"/> instance</returns>
        /// <exception cref="InvalidOperationException">thrown if the file is already opened. Use <see cref="IsFileOpen"/> to check this before.</exception>
        public IDataFile OpenFile(string path)
        {
            if (IsFileOpen(path))
            {
                throw new InvalidOperationException("The file has already been opened.");
            }

            IDataFile file = new XmlDataFile();
            file.Initialize(App);
            file.Open(Path.GetFullPath(path)); // todo: use MakeAbsolutePath() (?)

            filesOpen.Add(file);
            pathsOpen.Add(file.FilePath);
            dirList.Add(Path.GetDirectoryName(file.FilePath));

            foreach (string dirPath in file.DirectoryReferences)
            {
                if (!IsDirectoryOpen(dirPath))
                    OpenDirectory(Path.GetFullPath(dirPath));
            }
            foreach (string filePath in file.FileReferences)
            {
                if (!IsFileOpen(filePath))
                    OpenFile(filePath);
            }
            LoadResources(file, null);

            return file;
        }

        /// <summary>
        /// opens a new directory, i.e. adding it ot the list of directories to use for files
        /// </summary>
        /// <param name="path">the path to the directory to open</param>
        /// <exception cref="DirectoryNotFoundException">thrown if the directory doesn't exist</exception>
        /// <exception cref="InvalidOperationException">thrown if the directory is already opened. Use <see cref="IsDirectoryOpen"/> to check that.</exception>
        public void OpenDirectory(string path)
        {
            if (!Directory.Exists(path)) // todo: check if it's a subdir of an already opened directory
                throw new DirectoryNotFoundException();
            if (IsDirectoryOpen(path))
                throw new InvalidOperationException("The directory has already been opened.");

            dirList.Add(path);
        }

        /// <summary>
        /// checks if a given file has already been opened
        /// </summary>
        /// <param name="path">the path to the file to chec</param>
        /// <returns>true if already opened, false otherwise</returns>
        public bool IsFileOpen(string path)
        {
            return pathsOpen.Contains(path);
        }

        /// <summary>
        /// checks if a given directory has already been opened
        /// </summary>
        /// <param name="path">the path to the directory to check</param>
        /// <returns></returns>
        public bool IsDirectoryOpen(string path)
        {
            return dirList.Contains(path);
        }

        /// <summary>
        /// removes a file instance from the manager
        /// </summary>
        /// <param name="file"></param>
        public void Remove(IDataFile file)
        {
            file.Shutdown();

            filesOpen.Remove(file);
            pathsOpen.Remove(file.FilePath);
        }

        /// <summary>
        /// shuts down the manager
        /// </summary>
        public void Shutdown()
        {
            SaveAll();
            RemoveAll();

            App = null;
        }

        #region handle all

        private void LoadResources(IDataFile file, Resources.IResource parent)
        {
            foreach (var attributes in file.ResourceParseChildren(parent))
            {
                Guid type; Resources.IResource resource = null;

                if (Guid.TryParse(attributes["type"], out type))
                {
                    resource = App.ResourceTypeMan.CreateInstanceOf(type, attributes, parent, file); // try to use the element's name as resource alias
                }
                else if (Guid.TryParse(attributes["fallback"], out type))
                {
                    resource = App.ResourceTypeMan.CreateInstanceOf(type, attributes, parent, file); // give it a "2nd chance"
                }

                if (resource == null) // if creation failed:
                {
                    ChameleonCoderApp.Log("FileManager --> private void LoadResources(IDataFile, IResource)",
                        "failed to create resource",
                        "resource-creation failed in " + file.FilePath); // log
                    return; // ignore
                }

                App.ResourceMan.Add(resource, parent);
                LoadResources(file, resource);
            }
        }

        /// <summary>
        /// saves all opened instances
        /// </summary>
        public void SaveAll()
        {
            foreach (IDataFile file in Files)
                file.Save();
        }

        /// <summary>
        /// closes all opened instances
        /// </summary>
        public void RemoveAll()
        {
            foreach (IDataFile file in Files)
                Remove(file);

            filesOpen.Clear();
            pathsOpen.Clear();
            dirList.Clear();
        }

        #endregion // "handle all"

        #region properties

        /// <summary>
        /// a list of all referenced directories
        /// </summary>
        public string[] Directories
        {
            get
            {
                return dirList.ToArray();
            }
        }

        /// <summary>
        /// a list of all opened IDataFile instances
        /// </summary>
        public IDataFile[] Files
        {
            get
            {
                return filesOpen.ToArray();
            }
        }

        /// <summary>
        /// a list of the paths of all opened files
        /// </summary>
        public string[] FilePaths
        {
            get
            {
                return pathsOpen.ToArray();
            }
        }

        /// <summary>
        /// a reference to the App that created this instance
        /// </summary>
        public IChameleonCoderApp App
        {
            get;
            private set;
        }

        #endregion // "properties"

        /// <summary>
        /// tries to find the absolute path for a given relative path
        /// </summary>
        /// <param name="relativePath">the (relative) path</param>
        /// <returns>the absolute path, or null if not found</returns>
        public string MakeAbsolutePath(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                if (File.Exists(relativePath) || Directory.Exists(relativePath))
                {
                    if (Path.IsPathRooted(relativePath))
                        return relativePath;
                    return Path.GetFullPath(relativePath);
                }

                foreach (var dir in Directories)
                {
                    string fullpath = Path.Combine(dir, relativePath);
                    if (File.Exists(fullpath) || Directory.Exists(fullpath))
                        return fullpath;
                }
            }
            return null;
        }

        #region private fields

        /// <summary>
        /// a list of referenced directories
        /// </summary>
        private readonly List<string> dirList = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their file paths
        /// </summary>
        private readonly List<string> pathsOpen = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their IDataFile instances
        /// </summary>
        private readonly List<IDataFile> filesOpen = new List<IDataFile>();

        #endregion // "private fields"
    }
}
