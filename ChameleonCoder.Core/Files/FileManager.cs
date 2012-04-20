using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Files
{
    /// <summary>
    /// a class to manage the opened files
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), Guid("058BDE94-D8FA-4867-BFF6-98E1CD16470D")]
    public sealed class FileManager
    {
        /// <summary>
        /// creates a new instance of the class
        /// </summary>
        /// <param name="app">the app instrance the manager belongs to</param>
        internal FileManager(ChameleonCoderApp app)
        {
            App = app;
        }

        /// <summary>
        /// opens a given file
        /// </summary>
        /// <param name="path">the path to the file to open</param>
        /// <returns>the opened file as <see cref="DataFile"/> instance</returns>
        /// <exception cref="InvalidOperationException">thrown if the file is already opened. Use <see cref="IsOpen"/> to check this before.</exception>
        public DataFile Open(string path)
        {
            if (IsOpen(path))
            {
                throw new InvalidOperationException("The file has already been opened.");
            }

            DataFile file = new DataFile(Path.GetFullPath(path), App); // use MakeAbsolutePath() (?)

            filesOpen.Add(file);
            pathsOpen.Add(file.FilePath);
            dirList.Add(Path.GetDirectoryName(file.FilePath));

            return file;
        }

        /// <summary>
        /// opens a new directory, i.e. adding it ot the list of directories to use for files
        /// </summary>
        /// <param name="path">the path to the directory to use</param>
        /// <exception cref="DirectoryNotFoundException">thrown if the directory doesn't exist</exception>
        public void OpenDirectory(string path)
        {
            if (Directory.Exists(path))
                dirList.Add(path);
            // todo: check if already opened
            throw new DirectoryNotFoundException();
        }

        /// <summary>
        /// checks if a given file has already been opened
        /// </summary>
        /// <param name="path">the path to the file to chec</param>
        /// <returns>true if already opened, false otherwise</returns>
        public bool IsOpen(string path)
        {
            return pathsOpen.Contains(path); // todo: add possibly support for directories
        }

        /// <summary>
        /// removes a file instance from the manager
        /// </summary>
        /// <param name="file"></param>
        public void Remove(DataFile file)
        {
            filesOpen.Remove(file);
            pathsOpen.Remove(file.FilePath);
        }

        /// <summary>
        /// shuts down the manager
        /// </summary>
        public void Shutdown()
        {
            SaveAll();
            CloseAll();

            App = null;
        }

        #region handle all

        /// <summary>
        /// loads the resources in all opened files
        /// </summary>
        public void LoadAll()
        {
            foreach (DataFile file in Files)
            {
                if (!file.IsLoaded)
                    file.Load();
            }
        }

        /// <summary>
        /// saves all opened instances
        /// </summary>
        public void SaveAll()
        {
            foreach (DataFile file in Files)
                file.Save();
        }

        /// <summary>
        /// closes all opened instances
        /// </summary>
        public void CloseAll()
        {
            foreach (DataFile file in Files)
                file.Close();

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
        /// a list of all opened DataFile instances
        /// </summary>
        public DataFile[] Files
        {
            get
            {
                return filesOpen.ToArray();
            }
        }

        /// <summary>
        /// a list of the paths of all opened files
        /// </summary>
        public string[] Paths
        {
            get
            {
                return pathsOpen.ToArray();
            }
        }

        /// <summary>
        /// a reference to the App that created this instance
        /// </summary>
        public ChameleonCoderApp App
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
        [ComVisible(false)]
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
        [ComVisible(false)]
        private readonly List<string> dirList = new List<string>(new string[1] { Environment.CurrentDirectory }); // todo: remove currentDir and call OpenDirectory() from Main() instead

        /// <summary>
        /// contains a list of all loaded files in form of their file paths
        /// </summary>
        [ComVisible(false)]
        private readonly List<string> pathsOpen = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their DataFile instances
        /// </summary>
        [ComVisible(false)]
        private readonly List<DataFile> filesOpen = new List<DataFile>();

        #endregion // "private fields"
    }
}
