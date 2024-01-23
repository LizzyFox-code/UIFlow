namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using ViewModels;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class LayoutTable
    {
        public const int MaxLayoutCount = 32;
        
        private readonly UILayoutProxy[] m_RegisteredLayouts;
        private readonly Dictionary<Type, UILayoutId> m_TypeToLayoutIdMap;

        private int m_LayoutCount;

        public LayoutTable()
        {
            m_RegisteredLayouts = new UILayoutProxy[MaxLayoutCount];
            m_TypeToLayoutIdMap = new Dictionary<Type, UILayoutId>();
        }
        
        public UILayoutProxy FindLayout(in UILayoutId layoutId)
        {
            if (m_RegisteredLayouts[layoutId] == null)
                return UILayoutProxy.Invalid;
            
            return m_RegisteredLayouts[layoutId];
        }

        public UILayoutProxy FindLayoutWithContentType([NotNull] Type contentType)
        {
            for (var i = 0; i < m_RegisteredLayouts.Length; i++)
            {
                var layout = m_RegisteredLayouts[i];
                if (layout.HasContent(contentType))
                    return layout;
            }
            
            return UILayoutProxy.Invalid;
        }
        
        public UILayoutId RegisterLayout([NotNull] ILayoutViewModel viewModel, [NotNull]string name)
        {
            UILayoutId layoutId;
            if (m_LayoutCount >= MaxLayoutCount)
            {
                if (!TryFindEmptyLayoutId(out layoutId))
                    return UILayoutId.Invalid;
            }
            else
            {
                layoutId = new UILayoutId(m_LayoutCount++);
            }
            
            var layout = new UILayoutProxy(viewModel, name);
            m_RegisteredLayouts[layoutId] = layout;
            m_TypeToLayoutIdMap.Add(viewModel.GetType(), layoutId);
            
            return layoutId;
        }

        public bool TryUnregisterLayout(in UILayoutId layoutId, out UILayoutProxy layoutProxy)
        {
            if (m_RegisteredLayouts[layoutId] == null || layoutId < 7)
            {
                layoutProxy = default;
                return false;
            }

            layoutProxy = m_RegisteredLayouts[layoutId];
            m_RegisteredLayouts[layoutId] = null;
            m_TypeToLayoutIdMap.Remove(layoutProxy.ViewModelType);
            
            return true;
        }

        public UILayoutId GetLayoutId([NotNull] Type viewModelType)
        {
            if(!m_TypeToLayoutIdMap.TryGetValue(viewModelType, out var layoutId))
                return UILayoutId.Invalid;
            
            return layoutId;
        }

        internal bool TryGetLayoutId([NotNull] Type viewModelType, out UILayoutId layoutId)
        {
            return m_TypeToLayoutIdMap.TryGetValue(viewModelType, out layoutId);
        }
        
        private bool TryFindEmptyLayoutId(out UILayoutId layoutId)
        {
            layoutId = default;
            for (var i = 0; i < m_RegisteredLayouts.Length; i++)
            {
                if(m_RegisteredLayouts[i] != null)
                    continue;
                
                layoutId = new UILayoutId(i);
                return true;
            }

            return false;
        }
    }
}