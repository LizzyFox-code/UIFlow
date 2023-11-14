namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Noesis;

    public interface ILayoutViewModel
    {
        event LayoutChangedDelegate LayoutChanged;
        
        UILayoutId Id { get; set; }
        int Priority { get; set; }
        
        BaseLayoutContentViewModel Content { get; set; }

        void Set<TVm>(TVm item) where TVm : BaseLayoutContentViewModel;
        TVm Get<TVm>() where TVm : BaseLayoutContentViewModel;
        bool TryGet<TVm>(out TVm item) where TVm : BaseLayoutContentViewModel;
        
        void Add<T>([NotNull]T item, [NotNull]Type viewType) where T : BaseLayoutContentViewModel;
        void Remove<T>([NotNull]T item) where T : BaseLayoutContentViewModel;
        
        void RegisterView([NotNull]Type viewModelType, [NotNull]Type viewType);
        void UnregisterView([NotNull]Type viewModelType);
        
        DataTemplate FindTemplate([NotNull]Type viewModelType);
    }
}