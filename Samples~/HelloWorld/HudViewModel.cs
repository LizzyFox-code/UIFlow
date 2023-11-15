namespace Testing
{
    using System.Windows.Input;
    using UIFlow.Runtime;
    using UIFlow.Runtime.Layouts;
    using UIFlow.Runtime.Layouts.ViewModels;

    public sealed class HudViewModel : BaseLayoutContentViewModel
    {
        private ICommand m_Command;
        
        public ICommand ShowScreenCommand
        {
            get => m_Command;
            set => SetProperty(ref m_Command, value, nameof(ShowScreenCommand));
        }

        public HudViewModel()
        {
            m_Command = new Command(Show);
        }
        
        private void Show(object obj)
        {
            var viewModel = new ScreenViewModel();
            UIFlowUtility.ShowView<ScreenViewModel, ScreenView>(viewModel, UILayout.Screens);
        }
    }
}