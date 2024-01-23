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
        bool TryGet([NotNull]Type contentType, out BaseLayoutContentViewModel item);
        bool Has([NotNull]Type contentType);
        
        void Add([NotNull]BaseLayoutContentViewModel item, [NotNull]Type viewType);
        void Remove([NotNull]BaseLayoutContentViewModel item, bool unregisterTemplate = false);
        
        void RegisterView([NotNull]Type viewModelType, [NotNull]Type viewType);
        void UnregisterView([NotNull]Type viewModelType);
        
        DataTemplate FindTemplate([NotNull]Type viewModelType);
    }
}