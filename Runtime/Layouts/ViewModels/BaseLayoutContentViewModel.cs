namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class BaseLayoutContentViewModel : BaseViewModel
    {
        private static readonly string m_ShowCommandPropertyName = nameof(ShowCommand);
        private static readonly string m_HideCommandPropertyName = nameof(HideCommand);

        internal readonly HashSet<BaseLayoutContentViewModel> Dependencies;
        internal BaseLayoutContentViewModel Owner;
        
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
        
        public event LayoutContentEventHandler Showed;
        public event LayoutContentEventHandler Hidden;

        protected BaseLayoutContentViewModel()
        {
            Dependencies = new HashSet<BaseLayoutContentViewModel>();
            
            m_ShowCommand = new Command(_ => !m_IsShowed,_ => Show());
            m_HideCommand = new Command(_ => m_IsShowed, _ => Hide());
        }

        public void AddDependency([NotNull] BaseLayoutContentViewModel dependency)
        {
            if(!Dependencies.Add(dependency))
                Debug.LogWarning($"Dependency with type {dependency.GetType()} already exist.");

            dependency.Owner = this;
        }

        public void RemoveDependency([NotNull] BaseLayoutContentViewModel dependency)
        {
            if(!Dependencies.Remove(dependency))
                Debug.LogWarning($"Dependency with type {dependency.GetType()} doesn't exist.");

            dependency.Owner = null;
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