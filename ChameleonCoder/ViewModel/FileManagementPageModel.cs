using System.Collections.Generic;

namespace ChameleonCoder.ViewModel
{
    internal sealed class FileManagementPageModel : ViewModelBase
    {
        internal FileManagementPageModel(DataFile file)
        {
            this.file = file;
        }

        public static IList<DataFile> AllFiles
        {
            get
            {
                return DataFile.LoadedFiles;
            }
        }

        public IDictionary<string, string> Metadata
        {
            get
            {
                return file.GetMetadata();
            }
        }

        private readonly DataFile file;
    }
}
