namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using UnityEngine;

    public abstract class SingleContentLayoutViewModel : BaseLayoutViewModel
    {
        private static readonly string m_HistoryPropertyName = nameof(History);

        private ObservableCollection<BaseLayoutContentViewModel> m_History;
        private BaseLayoutContentViewModel m_CurrentItem;

        public ObservableCollection<BaseLayoutContentViewModel> History
        {
            get => m_History;
            set => SetProperty(ref m_History, value, m_HistoryPropertyName);
        }

        public override BaseLayoutContentViewModel Content
        {
            get => m_CurrentItem;
            set => SetContentInternal(ref m_CurrentItem, value);
        }

        protected SingleContentLayoutViewModel()
        {
            m_History = new ObservableCollection<BaseLayoutContentViewModel>();
        }
        
        public override void Set(BaseLayoutContentViewModel item)
        {
            Content = item;
        }

        public override TVm Get<TVm>()
        {
            var type = typeof(TVm);
            if(m_CurrentItem != null && m_CurrentItem.GetType() == type)
                return m_CurrentItem as TVm;
            
            return m_History.FirstOrDefault(x => x.GetType() == type) as TVm;
        }

        public override bool TryGet<TVm>(out TVm item)
        {
            var type = typeof(TVm);
            if(m_CurrentItem != null && m_CurrentItem.GetType() == type)
            {
                item = m_CurrentItem as TVm;
                return true;
            }
            
            item = m_History.FirstOrDefault(x => x.GetType() == type) as TVm;
            return item != null;
        }

        public override void Add(BaseLayoutContentViewModel item, Type viewType)
        {
            if (m_CurrentItem != null)
                History.Add(m_CurrentItem);

            RegisterView(item.GetType(), viewType);
            Set(item);
        }

        public override void Remove(BaseLayoutContentViewModel item, bool unregisterTemplate = false)
        {
            if (m_CurrentItem != item)
            {
                if(!History.Remove(item))
                    Debug.Log($"View model with type {item.GetType()} doesn't exist in layout.");
                
                if(unregisterTemplate)
                    UnregisterView(item.GetType());
                return;
            }
            
            var previousItem = PopFromHistory();
            Set(previousItem);
            if(unregisterTemplate)
                UnregisterView(item.GetType());
        }
        
        private BaseLayoutContentViewModel PopFromHistory()
        {
            if (History.Count == 0)
                return null;
            
            var item = History[^1];
            History.RemoveAt(History.Count - 1);
            return item;
        }
    }
}