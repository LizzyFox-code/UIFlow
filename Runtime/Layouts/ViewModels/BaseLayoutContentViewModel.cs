namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;
    using System.Windows.Input;

    public abstract class BaseLayoutContentViewModel : BaseViewModel
    {
        private static readonly string m_ShowCommandPropertyName = nameof(ShowCommand);
        private static readonly string m_HideCommandPropertyName = nameof(HideCommand);
        
        private bool m_IsShowed;

        private ICommand m_ShowCommand;
        private ICommand m_HideCommand;

        internal ICommand ShowCommand
        {
            get => m_ShowCommand;
            set => SetProperty(ref m_ShowCommand, value, m_ShowCommandPropertyName);
        }

        internal ICommand HideCommand
        {
            get => m_HideCommand;
            set => SetProperty(ref m_HideCommand, value, m_HideCommandPropertyName);
        }

        public bool IsShowed => m_IsShowed;
        
        public event Action<BaseLayoutContentViewModel> Showed;
        public event Action<BaseLayoutContentViewModel> Hidden;

        protected BaseLayoutContentViewModel()
        {
            m_ShowCommand = new Command(_ => !m_IsShowed,_ => Show());
            m_HideCommand = new Command(_ => m_IsShowed, _ => Hide());
        }
        
        private void Show()
        {
            if(m_IsShowed)
                return;
            
            m_IsShowed = true;
            
            OnBeforeShow();
            Showed?.Invoke(this);
            OnAfterShow();
        }

        private void Hide()
        {
            if(!m_IsShowed)
                return;
            
            m_IsShowed = false;
            
            OnBeforeHide();
            Hidden?.Invoke(this);
            OnAfterHide();
        }

        protected virtual void OnBeforeShow() { }
        protected virtual void OnAfterShow() { }
        protected virtual void OnBeforeHide() { }
        protected virtual void OnAfterHide() { }
    }
}