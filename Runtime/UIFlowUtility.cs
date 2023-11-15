namespace UIFlow.Runtime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Layouts;
    using Layouts.ViewModels;
    using Layouts.Views;
    using UnityEngine;

    public static class UIFlowUtility
    {
        private static readonly UIManagerFactory m_Factory = new UIManagerFactory();
        
        // ReSharper disable once InconsistentNaming
        internal static UIManager m_InternalManager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void Initialize()
        {
            var settingsAsset = UIFlowSettingsAsset.GetAsset();
#if DEBUG
            if(settingsAsset == null)
                throw new InvalidOperationException("UI Flow settings asset not found.");
#endif
            m_InternalManager = m_Factory.Create(settingsAsset);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UILayoutProxy FindLayout(in UILayoutId layoutId)
        {
            return m_InternalManager.FindLayout(layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<TVm, TV>([NotNull]TVm viewModel, in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.Show<TVm, TV>(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<T>([NotNull]T viewModel, in UILayoutId layoutId) where T : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.ShowContent(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HideView<T>([NotNull] T viewModel, in UILayoutId layoutId) where T : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.HideContent(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView<T>(in UILayoutId layoutId) where T : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            return layout.HideContent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterView<TVm, TV>(in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.RegisterView<TVm, TV>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterView<TVm>(in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if DEBUG
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.UnregisterView<TVm>();
        }
    }
}