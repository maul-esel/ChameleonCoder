namespace ChameleonCoder.Files
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal static class DocumentXPath
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

        [System.Obsolete]
        internal const string References = ReferenceRoot + "/" + prefix + "reference";

        #endregion

        internal const string ResourceRoot = Root + "/" + prefix + "resources";
        internal const string Resources = ResourceRoot + "/" + prefix + "resource";

        internal const string DataRoot = Root + "/" + prefix + "data";
        internal const string ResourceData = DataRoot + "/" + prefix + "resourcedata";
    }
}
