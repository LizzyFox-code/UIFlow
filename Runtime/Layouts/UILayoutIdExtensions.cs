namespace UIFlow.Runtime.Layouts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Pool;
    using ViewModels;
    using Views;

    public static class UILayoutIdExtensions
    {
#if UNITY_EDITOR
        private static UIFlowSettingsAsset m_SettingsAsset;
#endif
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UILayoutProxy FindLayout(this UILayoutId layoutId)
        {
            return UIFlowUtility.FindLayout(layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<TVm, TV>(this UILayoutId layoutId, [NotNull]TVm viewModel) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            UIFlowUtility.ShowView<TVm, TV>(viewModel, layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<T>(this UILayoutId layoutId, [NotNull]T viewModel) where T : BaseLayoutContentViewModel
        {
            UIFlowUtility.ShowView(viewModel, layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HideView<T>(this UILayoutId layoutId, [NotNull] T viewModel) where T : BaseLayoutContentViewModel
        {
            UIFlowUtility.HideView(viewModel, layoutId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView<T>(this UILayoutId layoutId) where T : BaseLayoutContentViewModel
        {
            return UIFlowUtility.HideView<T>(layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterView<TVm, TV>(this UILayoutId layoutId) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            UIFlowUtility.RegisterView<TVm, TV>(layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterView<TVm>(this UILayoutId layoutId) where TVm : BaseLayoutContentViewModel
        {
            UIFlowUtility.UnregisterView<TVm>(layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string LayoutToName(this UILayoutId layoutId)
        {
            if(!Application.isPlaying || Application.isEditor)
            {
                var settingsAsset = GetOrFindSettingsAsset();
                if (settingsAsset == null || settingsAsset.LayoutCount <= layoutId)
                    return string.Empty;

                var layouts = ListPool<UILayoutData>.Get();
                settingsAsset.GetLayouts(layouts);
                var name = layouts[layoutId].Name;
                ListPool<UILayoutData>.Release(layouts);
                return name;
            }
            
            var uiLayout = UIFlowUtility.FindLayout(layoutId);
            return uiLayout == null ? string.Empty : uiLayout.Name;
        }
        
        private static UIFlowSettingsAsset GetOrFindSettingsAsset()
        {
#if UNITY_EDITOR
            if (m_SettingsAsset != null)
                return m_SettingsAsset;
                
            var assetGuid = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(UIFlowSettingsAsset)}").FirstOrDefault();
            if(string.IsNullOrEmpty(assetGuid))
                return null;
                
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(assetGuid);
            if(string.IsNullOrEmpty(assetPath))
                return null;
                
            m_SettingsAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UIFlowSettingsAsset>(assetPath);
            return m_SettingsAsset;
#else
            return null;
#endif
        }
    }
}