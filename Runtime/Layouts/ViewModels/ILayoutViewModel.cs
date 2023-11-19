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

        void Set(BaseLayoutContentViewModel item);
        TVm Get<TVm>() where TVm : BaseLayoutContentViewModel;
        bool TryGet<TVm>(out TVm item) where TVm : BaseLayoutContentViewModel;
        
        void Add([NotNull]BaseLayoutContentViewModel item, [NotNull]Type viewType);
        void Remove([NotNull]BaseLayoutContentViewModel item);
        
        void RegisterView([NotNull]Type viewModelType, [NotNull]Type viewType);
        void UnregisterView([NotNull]Type viewModelType);
        
        DataTemplate FindTemplate([NotNull]Type viewModelType);
    }
}