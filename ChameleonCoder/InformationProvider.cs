
namespace ChameleonCoder
{
    public static class InformationProvider
    {
        public static string ProgrammingDirectory
        {
            get
            {
                return Properties.Settings.Default.ScriptDir;
            }
        }
    }
}
