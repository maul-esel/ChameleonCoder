namespace ChameleonCoder.Navigation
{
    internal abstract class CCPageBase : System.Windows.Controls.Page
    {
        public CCPageBase()
        {
        }

        protected CCPageBase(ViewModel.ViewModelBase model)
        {
            DataContext = model;
            CommandBindings.AddRange(model.Commands);
        }
    }
}
