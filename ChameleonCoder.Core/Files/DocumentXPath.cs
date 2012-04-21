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

        internal const string ReferenceRoot = Settings + "/" + prefix + "references";
        internal const string References = ReferenceRoot + "/" + prefix + "reference";

        internal const string ResourceRoot = Root + "/" + prefix + "resources";
        internal const string Resources = ResourceRoot + "/" + prefix + "resource";

        internal const string DataRoot = Root + "/" + prefix + "data";
        internal const string ResourceData = DataRoot + "/" + prefix + "resourcedata";
    }
}
