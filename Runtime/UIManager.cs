namespace UIFlow.Runtime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Layouts;
    using Layouts.ViewModels;
    using Noesis;
    using ViewModels;

    internal sealed class UIManager
    {
#pragma warning disable CS0169
        private NoesisView m_NoesisView;
#pragma warning restore CS0169

        private readonly FrameworkElement m_View;
        private readonly IUIContainerViewModel m_ViewModel;

        private readonly LayoutTable m_LayoutTable;
        private readonly LayoutRelationshipTable m_RelationshipTable;

        public UIManager([NotNull]NoesisView noesisView, [NotNull]IUIContainerViewModel viewModel, [NotNull]LayoutTable layoutTable, [NotNull]LayoutRelationshipTable relationshipTable)
        {
            m_NoesisView = noesisView;
            
            m_View = noesisView.Content;
            m_View.DataContext = viewModel;
            
            m_ViewModel = viewModel;
            m_LayoutTable = layoutTable;
            m_RelationshipTable = relationshipTable;
        }
        
        public UILayoutProxy FindLayout(in UILayoutId layoutId)
        {
            return m_LayoutTable.FindLayout(layoutId);
        }
        
        public UILayoutId RegisterLayout([NotNull]ILayoutViewModel viewModel, [NotNull]Type viewType, int priority, in UILayoutMask mask, [NotNull]string name)
        {
            var layoutId = m_LayoutTable.RegisterLayout(viewModel, name);
            if (!layoutId.IsValid)
                return layoutId;
            
            viewModel.Id = layoutId;
            viewModel.Priority = priority;
            
            m_RelationshipTable.SetMask(layoutId, mask);
            
            var dataTemplate = DataTemplateUtility.CreateTemplate(viewModel.GetType(), viewType);
            m_View.Resources.Add(dataTemplate.DataType, dataTemplate);
            m_ViewModel.AddLayout(viewModel, priority);
            
            return layoutId;
        }

        public void UnregisterLayout(in UILayoutId layoutId)
        {
            if (!m_LayoutTable.TryUnregisterLayout(layoutId, out var layoutProxy))
                return;
            
            m_ViewModel.RemoveLayout(layoutProxy.ViewModel);
            m_View.Resources.Remove(layoutProxy.ViewModelType);
            
            m_RelationshipTable.ClearMask(layoutId);
        }
    }
}