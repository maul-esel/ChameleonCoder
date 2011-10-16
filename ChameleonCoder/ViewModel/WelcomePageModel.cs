using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings for the WelcomePage class
    /// </summary>
    [DefaultRepresentation(typeof(Navigation.WelcomePage))]
    internal sealed class WelcomePageModel : ViewModelBase
    {
        private WelcomePageModel()
        {
        }

        internal static WelcomePageModel Instance
        {
            get
            {
                lock (modelInstance)
                {
                    return modelInstance;
                }
            }
        }

        private static readonly WelcomePageModel modelInstance = new WelcomePageModel();

        public static string CreateResource { get { return Res.WP_CreateResource; } }

        public static string GoList { get { return Res.WP_GoList; } }

        public static string GoPlugins { get { return Res.WP_GoPlugins; } }

        public static string GoSettings { get { return Res.WP_GoSettings; } }

        public static string StartSelection { get { return Res.WP_StartSelection; } }

        public static string Welcome { get { return Res.WP_Welcome; } }
    }
}
