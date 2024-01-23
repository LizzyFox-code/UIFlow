namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Noesis;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class BaseLayoutViewModel : BaseViewModel, ILayoutViewModel
    {
        private static readonly string m_CurrentItemPropertyName = nameof(Content);
        private static readonly string m_IdPropertyName = nameof(Id);
        private static readonly string m_PriorityPropertyName = nameof(Priority);
    
        private readonly Dictionary<Type, DataTemplate> m_Templates;
        
        private UILayoutId m_Id;
        private int m_Priority;

        public virtual BaseLayoutContentViewModel Content { get; set; }

        public UILayoutId Id
        {
            get => m_Id;
            set => SetProperty(ref m_Id, value, m_IdPropertyName);
        }

        public int Priority
        {
            get => m_Priority;
            set => SetProperty(ref m_Priority, value, m_PriorityPropertyName);
        }
        
        public event LayoutChangedDelegate LayoutChanged;

        protected BaseLayoutViewModel()
        {
            m_Templates = new Dictionary<Type, DataTemplate>();
        }

        public abstract void Set(BaseLayoutContentViewModel item);
        public abstract TVm Get<TVm>() where TVm : BaseLayoutContentViewModel;
        public abstract bool TryGet([NotNull]Type contentType, out BaseLayoutContentViewModel item);
        public abstract bool Has(Type contentType);

        public abstract void Add(BaseLayoutContentViewModel item, Type viewType);
        public abstract void Remove(BaseLayoutContentViewModel item, bool unregisterTemplate = false);

        public void RegisterView(Type viewModelType, Type viewType)
        {
            if(m_Templates.ContainsKey(viewModelType))
                return;

            var dataTemplate = DataTemplateUtility.CreateTemplate(viewModelType, viewType);
            m_Templates.Add(dataTemplate.DataType, dataTemplate);
        }

        public void UnregisterView(Type viewModelType)
        {
            m_Templates.Remove(viewModelType);
        }
        
        public DataTemplate FindTemplate(Type viewModelType)
        {
            return m_Templates.GetValueOrDefault(viewModelType);
        }

        protected void SetContentInternal([NotNull]ref BaseLayoutContentViewModel target, BaseLayoutContentViewModel newValue = null)
        {
            var oldValue = target;
            SetProperty(ref target, newValue, m_CurrentItemPropertyName);
            LayoutChanged?.Invoke(this, oldValue, newValue);
        }
    }
}