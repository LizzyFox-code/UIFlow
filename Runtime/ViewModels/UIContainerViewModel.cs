namespace UIFlow.Runtime.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using Layouts;
    using Layouts.ViewModels;

    internal sealed class UIContainerViewModel : BaseViewModel, IUIContainerViewModel
    {
        private static readonly string m_LayoutsPropertyName = nameof(Layouts);
        private static readonly string m_LayoutMaskPropertyName = nameof(LayoutMask);
        
        private readonly LayoutTable m_LayoutTable;
        private readonly LayoutRelationshipTable m_RelationshipTable;

        private readonly List<int> m_Priorities;

        private ObservableCollection<ILayoutViewModel> m_Layouts;
        private UILayoutMask m_LayoutMask = UILayoutMask.One;

        public ObservableCollection<ILayoutViewModel> Layouts
        {
            get => m_Layouts;
            set => SetProperty(ref m_Layouts, value, m_LayoutsPropertyName);
        }

        public UILayoutMask LayoutMask
        {
            get => m_LayoutMask;
            set => SetProperty(ref m_LayoutMask, value, m_LayoutMaskPropertyName);
        }

        public UIContainerViewModel([NotNull]LayoutTable layoutTable, [NotNull]LayoutRelationshipTable relationshipTable)
        {
            m_LayoutTable = layoutTable;
            m_RelationshipTable = relationshipTable;
            
            m_Priorities = new List<int>();
            m_Layouts = new ObservableCollection<ILayoutViewModel>();
        }

        public void AddLayout(ILayoutViewModel viewModel, int priority)
        {
            Subscribe(viewModel);
            if (TryFindIndexByPriority(priority, out var index))
            {
                m_Priorities.Insert(index, priority);
                m_Layouts.Insert(index, viewModel);
            }
            else
            {
                m_Priorities.Add(priority);
                m_Layouts.Add(viewModel);
            }
        }

        public void RemoveLayout(ILayoutViewModel viewModel)
        {
            m_Layouts.Remove(viewModel);
            Unsubscribe(viewModel);
        }

        private void Subscribe([NotNull]ILayoutViewModel viewModel)
        {
            viewModel.LayoutChanged += OnLayoutChanged;
        }

        private void Unsubscribe([NotNull]ILayoutViewModel viewModel)
        {
            viewModel.LayoutChanged -= OnLayoutChanged;
        }
        
        private void OnLayoutChanged(ILayoutViewModel viewModel, BaseLayoutContentViewModel newContent)
        {
            if (!TryGetHighestPriorityViewModelWithContent(out var highestPriorityViewModel))
            {
                LayoutMask = UILayoutMask.Zero;
                return;
            }
            
            if(!m_LayoutTable.TryGetLayoutId(highestPriorityViewModel.GetType(), out var layoutId))
            {
                LayoutMask = UILayoutMask.Zero;
                return;
            }
            
            LayoutMask = m_RelationshipTable.GetMask(layoutId);
        }

        private bool TryGetHighestPriorityViewModelWithContent(out ILayoutViewModel viewModel)
        {
            var maxPriority = int.MinValue;
            viewModel = null;
            
            foreach (var layoutViewModel in m_Layouts)
            {
                if (layoutViewModel.Priority < maxPriority || layoutViewModel.Content == null) 
                    continue;
                
                maxPriority = layoutViewModel.Priority;
                viewModel = layoutViewModel;
            }
            
            return viewModel != null;
        }

        private bool TryFindIndexByPriority(int priority, out int index)
        {
            index = -1;
            for (var i = 0; i < m_Priorities.Count; i++)
            {
                if (m_Priorities[i] < priority) 
                    continue;
                
                index = i;
                return true;
            }

            return false;
        }
    }
}