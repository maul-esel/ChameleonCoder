namespace ChameleonCoder.SearchReplace
{
    interface ISearchReplaceClient
    {
        string GetText();

        void ReplaceText(int offset, int length, string replaceText);

        void SelectText(int offset, int length);
    }
}
