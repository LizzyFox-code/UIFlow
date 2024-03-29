namespace UIFlow.Runtime.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using Layouts;
    using Layouts.ViewModels;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
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
        
        private void OnLayoutChanged(ILayoutViewModel viewModel, BaseLayoutContentViewModel oldContent, BaseLayoutContentViewModel newContent)
        {
            if (!TryGetHighestPriorityViewModelWithContent(out var highestPriorityViewModel) ||
                 !m_LayoutTable.TryGetLayoutId(highestPriorityViewModel.GetType(), out var layoutId))
            {
                if(!TryWaitLastContentHiding(viewModel, oldContent))
                    LayoutMask = UILayoutMask.Zero;
                
                return;
            }

            if (highestPriorityViewModel != viewModel && viewModel.Priority < highestPriorityViewModel.Priority || newContent != null
            || !TryWaitOldContentHiding(viewModel, oldContent, layoutId))
            {
                LayoutMask = m_RelationshipTable.GetMask(layoutId);
            }
        }

        private bool TryWaitLastContentHiding(ILayoutViewModel viewModel, BaseLayoutContentViewModel content)
        {
            if (content == null || !content.IsShowed || !m_LayoutTable.TryGetLayoutId(viewModel.GetType(), out var layoutId))
                return false;
            
            LayoutMask = m_RelationshipTable.GetMask(layoutId);
            
            content.Hidden += Handler;
            
            void Handler(BaseLayoutContentViewModel model)
            {
                LayoutMask = LayoutMask = UILayoutMask.Zero;
                content.Hidden -= Handler;
            }
            return true;
        }

        private bool TryWaitOldContentHiding(ILayoutViewModel viewModel, BaseLayoutContentViewModel oldContent, UILayoutId layoutId)
        {
            if (oldContent == null || !oldContent.IsShowed || !m_LayoutTable.TryGetLayoutId(viewModel.GetType(), out var oldLayoutId))
                return false;

            LayoutMask = m_RelationshipTable.GetMask(layoutId) | m_RelationshipTable.GetMask(oldLayoutId);

            oldContent.Hidden += Handler;
            
            void Handler(BaseLayoutContentViewModel model)
            {
                LayoutMask = m_RelationshipTable.GetMask(layoutId);
                oldContent.Hidden -= Handler;
            }
            return true;
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