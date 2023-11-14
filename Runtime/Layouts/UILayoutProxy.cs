namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ViewModels;

    public sealed class UILayoutProxy
    {
        public static readonly UILayoutProxy Invalid = new UILayoutProxy(null, string.Empty);
        
        internal readonly ILayoutViewModel ViewModel;

        internal readonly Type ViewModelType;
        public readonly bool IsValid;
        public readonly string Name;

        internal UILayoutProxy(ILayoutViewModel viewModel, string name)
        {
            ViewModel = viewModel;
            ViewModelType = viewModel?.GetType();
            IsValid = viewModel != null;
            Name = name;
        }

        public void ShowContent<T>([NotNull]T item) where T : BaseLayoutContentViewModel
        {
            ViewModel.Set(item);
        }

        public void ShowContent<T>([NotNull] T item, Type viewType) where T : BaseLayoutContentViewModel
        {
            ViewModel.Add(item, viewType);
        }

        public void HideContent<T>([NotNull] T item) where T : BaseLayoutContentViewModel
        {
            ViewModel.Remove(item);
        }

        public bool HideContent<T>() where T : BaseLayoutContentViewModel
        {
            if(!ViewModel.TryGet<T>(out var item))
                return false;
            
            ViewModel.Remove(item);
            return true;
        }
        
        public void RegisterContentViewType(Type viewModelType, Type viewType)
        {
            ViewModel.RegisterView(viewModelType, viewType);
        }

        public void UnregisterContentViewType(Type viewModelType)
        {
            ViewModel.UnregisterView(viewModelType);
        }
    }
}