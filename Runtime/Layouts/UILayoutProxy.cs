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

        public void ShowContent([NotNull]BaseLayoutContentViewModel item)
        {
            ViewModel.Set(item);
        }

        public void ShowContent([NotNull] BaseLayoutContentViewModel item, Type viewType)
        {
            ViewModel.Add(item, viewType);
        }

        public void HideContent([NotNull] BaseLayoutContentViewModel item, bool unregisterTemplate = false)
        {
            ViewModel.Remove(item, unregisterTemplate);
        }

        public bool HideContent<T>(bool unregisterTemplate = false) where T : BaseLayoutContentViewModel
        {
            if(!ViewModel.TryGet<T>(out var item))
                return false;
            
            ViewModel.Remove(item, unregisterTemplate);
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