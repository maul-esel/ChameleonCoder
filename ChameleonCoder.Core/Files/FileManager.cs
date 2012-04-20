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
        [DispId(1)]
        public DataFile Open(string path)
        {
            if (IsOpen(path))
            {
                throw new InvalidOperationException("The file has already been opened.");
            }

            DataFile file = new DataFile(Path.GetFullPath(path), App); // use MakeAbsolutePath() (?)

            Files.Add(file);
            pathsOpen.Add(file.FilePath);
            Directories.Add(Path.GetDirectoryName(file.FilePath));

            return file;
        }

        /// <summary>
        /// checks if a given file has already been opened
        /// </summary>
        /// <param name="path">the path to the file to chec</param>
        /// <returns>true if already opened, false otherwise</returns>
        [DispId(2)]
        public bool IsOpen(string path)
        {
            return pathsOpen.Contains(path);
        }

        #region handle all

        /// <summary>
        /// loads the resources in all opened files
        /// </summary>
        [DispId(3)]
        public void LoadAll()
        {
            foreach (var file in Files)
            {
                if (!file.IsLoaded)
                    file.Load();
            }
        }

        /// <summary>
        /// saves all opened instances
        /// </summary>
        [DispId(4)]
        public void SaveAll()
        {
            foreach (DataFile file in Files)
                file.Save();
        }

        /// <summary>
        /// closes all opened instances
        /// </summary>
        [DispId(5)]
        public void CloseAll()
        {
            foreach (DataFile file in Files)
                file.Close();

            Files.Clear();
            Paths.Clear();
            Directories.Clear();
        }

        #endregion // "handle all"

        #region properties

        /// <summary>
        /// a list of all referenced directories
        /// </summary>
        [DispId(6)]
        public IList<string> Directories
        {
            get
            {
                return dirList;
            }
        }

        /// <summary>
        /// a list of all opened DataFile instances
        /// </summary>
        [DispId(7)]
        public IList<DataFile> Files
        {
            get
            {
                return filesOpen;
            }
        }

        /// <summary>
        /// a list of the paths of all opened files
        /// </summary>
        [DispId(8)]
        public IList<string> Paths
        {
            get
            {
                return pathsOpen;
            }
        }

        /// <summary>
        /// a reference to the App that created this instance
        /// </summary>
        [DispId(9)]
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
        private readonly List<string> dirList = new List<string>(new string[1] { Environment.CurrentDirectory });

        /// <summary>
        /// contains a list of all loaded files in form of their file paths
        /// </summary>
        [ComVisible(false)]
        private readonly IList<string> pathsOpen = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their DataFile instances
        /// </summary>
        [ComVisible(false)]
        private readonly IList<DataFile> filesOpen = new List<DataFile>();

        #endregion // private fields
    }
}
