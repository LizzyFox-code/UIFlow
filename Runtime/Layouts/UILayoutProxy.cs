namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine.Pool;
    using ViewModels;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UILayoutProxy
    {
        public static readonly UILayoutProxy Invalid = new(null, string.Empty);
        
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
            
            HideAllDependencies(item, unregisterTemplate);
            item.Owner?.RemoveDependency(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HideContent(Type contentType, bool unregisterTemplate = false)
        {
            if(!ViewModel.TryGet(contentType, out var item))
                return false;
            
            ViewModel.Remove(item, unregisterTemplate);
            
            HideAllDependencies(item, unregisterTemplate);
            item.Owner?.RemoveDependency(item);
            
            return true;
        }

        public bool HasContent([NotNull]Type contentType)
        {
            return ViewModel.Has(contentType);
        }
        
        public void RegisterContentViewType(Type viewModelType, Type viewType)
        {
            ViewModel.RegisterView(viewModelType, viewType);
        }

        public void UnregisterContentViewType(Type viewModelType)
        {
            ViewModel.UnregisterView(viewModelType);
        }

        private void HideAllDependencies([NotNull] BaseLayoutContentViewModel item, bool unregisterTemplate = false)
        {
            var dependenciesCopy = HashSetPool<BaseLayoutContentViewModel>.Get();
            foreach (var dependency in item.Dependencies)
            {
                dependenciesCopy.Add(dependency);
            }
            
            foreach (var dependency in dependenciesCopy)
            {
                HideContent(dependency, unregisterTemplate);
            }
            item.Dependencies.Clear();
            HashSetPool<BaseLayoutContentViewModel>.Release(dependenciesCopy);
        }
    }
}