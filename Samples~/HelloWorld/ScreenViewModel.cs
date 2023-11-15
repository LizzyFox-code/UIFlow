namespace Testing
{
    using System.Windows.Input;
    using UIFlow.Runtime;
    using UIFlow.Runtime.Layouts;
    using UIFlow.Runtime.Layouts.ViewModels;

    public sealed class ScreenViewModel : BaseLayoutContentViewModel
    {
        private ICommand m_Command;
        
        public ICommand HideScreenCommand
        {
            get => m_Command;
            set => SetProperty(ref m_Command, value, nameof(HideScreenCommand));
        }

        public ScreenViewModel()
        {
            m_Command = new Command(Show);
        }
        
        private void Show(object obj)
        {
            UIFlowUtility.HideView(this, UILayout.Screens);
        }
    }
}