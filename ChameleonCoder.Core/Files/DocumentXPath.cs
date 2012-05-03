namespace ChameleonCoder.Files
{
    public partial class XmlDataFile
    {
        [System.Runtime.InteropServices.ComVisible(false)]
        internal static class DocumentXPath // todo: make private subclass of XmlDataFile
        {
            internal const string prefix = "cc:";
            internal const string Root = "/" + prefix + "ChameleonCoder";

            internal const string Settings = Root + "/" + prefix + "settings";

            internal const string SettingName = Settings + "/" + prefix + "name";

            internal const string MetadataRoot = Settings + "/" + prefix + "metadata";
            internal const string Metadata = MetadataRoot + "/" + prefix + "metadata";

            #region DataFile references

            internal const string ReferenceRootNode = prefix + "references";
            internal const string ReferenceRoot = Settings + "/" + ReferenceRootNode;

            internal const string FileReferenceNode = prefix + "file";
            internal const string FileReferenceList = ReferenceRoot + "/" + FileReferenceNode;

            internal const string DirectoryReferenceNode = prefix + "directory";
            internal const string DirectoryReferenceList = ReferenceRoot + "/" + DirectoryReferenceNode;

            #endregion

            internal const string ResourceRoot = Root + "/" + prefix + "resources";
            internal const string Resources = ResourceRoot + "/" + prefix + "resource";

            internal const string DataRoot = Root + "/" + prefix + "data";

            internal const string ResourceDataNode = prefix + "resourcedata";
            internal const string ResourceDataList = DataRoot + "/" + ResourceDataNode;

            internal const string RichContentNode = prefix + "richcontent";
            internal const string RichContentList = DataRoot + "/" + RichContentNode;

            internal const string ResourceReferenceNode = prefix + "reference";
            internal const string ResourceReferenceSubpath = prefix + "references/" + ResourceReferenceNode;
        }
    }
}
