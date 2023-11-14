namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using ViewModels;
    
    [Serializable]
    public sealed class UILayoutData
    {
        [SerializeField]
        private string m_Name = "Layout";
        [SerializeField]
        private int m_Priority;
        [SerializeField]
        private NoesisXaml m_Xaml;
        [SerializeReference]
        private ILayoutViewModel m_ViewModel;

        public string Name => m_Name;
        
        public NoesisXaml Xaml => m_Xaml;
        public ILayoutViewModel ViewModel => m_ViewModel;

        public int Priority => m_Priority;

        public UILayoutData(){}
        
        public UILayoutData([NotNull]string name, int priority, [NotNull]NoesisXaml xaml, [NotNull]ILayoutViewModel viewModel)
        {
            m_Name = name;
            m_Priority = priority;
            m_Xaml = xaml;
            m_ViewModel = viewModel;
        }
    }
}