namespace ChameleonCoder.Files
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal static class DocumentXPath
    {
        internal const string prefix = "cc:";
        internal const string Root = "/" + prefix + "ChameleonCoder";

        internal const string Settings = Root + "/" + prefix + "settings";

        internal const string SettingName = Settings + "/" + prefix + "name";

        internal const string References = Settings + "/" + prefix + "references";
        internal const string SingleReference = References + "/" + prefix + "reference";

        internal const string Resources = Root + "/" + prefix + "resources";
        internal const string SingleResource = Resources + "/" + prefix + "resource";
    }
}
