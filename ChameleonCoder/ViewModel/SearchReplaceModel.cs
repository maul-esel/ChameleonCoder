using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings for the CCSearchReplace dialog
    /// </summary>
    [DefaultRepresentation(typeof(CCSearchReplaceDialog))]
    internal sealed class SearchReplaceModel : ViewModelBase
    {
        internal SearchReplaceModel(ChameleonCoderApp app)
            : base(app)
        {
        }

        #region localization

        public static string SearchFor { get { return Res.SR_SearchFor; } }

        public static string ReplaceBy { get { return Res.SR_ReplaceBy; } }

        public static string FindNext { get { return Res.SR_FindNext; } }

        public static string Replace { get { return Res.Edit_Replace; } }

        public static string ReplaceAll { get { return Res.SR_ReplaceAll; } }

        public static string Title { get { return Res.SR_Title; } }

        public static string MatchCase { get { return Res.SR_MatchCase; } }

        public static string MatchWholeWord { get { return Res.SR_MatchWholeWord; } }

        public static string WrapAround { get { return Res.SR_WrapAround; } }

        #endregion // "localization"
    }
}
