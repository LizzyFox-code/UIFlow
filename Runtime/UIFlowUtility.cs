namespace UIFlow.Runtime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Layouts;
    using Layouts.ViewModels;
    using Layouts.Views;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class UIFlowUtility
    {
        private static readonly UIManagerFactory m_Factory = new();
        
        internal static UIManager m_InternalManager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            var settingsAsset = UIFlowSettingsAsset.GetAsset();
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(settingsAsset == null)
                throw new InvalidOperationException("UI Flow settings asset not found.");
#endif
            m_InternalManager = m_Factory.Create(settingsAsset);
        }

        /// <summary>
        /// Finds the <see cref="UILayoutProxy"/> associated with the given <paramref name="layoutId"/>.
        /// </summary>
        /// <param name="layoutId">The identifier of the layout.</param>
        /// <returns>The <see cref="UILayoutProxy"/> associated with the given <paramref name="layoutId"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UILayoutProxy FindLayout(in UILayoutId layoutId)
        {
            return m_InternalManager.FindLayout(layoutId);
        }

        /// <summary>
        /// Returns the layout ID associated with the specified view model type.
        /// </summary>
        /// <param name="viewModelType">The type of the view model.</param>
        /// <returns>The layout ID associated with the view model type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UILayoutId GetLayoutId([NotNull]Type viewModelType)
        {
            return m_InternalManager.GetLayoutId(viewModelType);
        }

        /// <summary>
        /// Gets the layout ID for a given type of view model that implements the <see cref="ILayoutViewModel"/> interface.
        /// </summary>
        /// <typeparam name="T">The type of view model</typeparam>
        /// <returns>The layout ID</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UILayoutId GetLayoutId<T>() where T : ILayoutViewModel
        {
            return m_InternalManager.GetLayoutId<T>();
        }

        /// <summary>
        /// Shows a view with the given view model and layout ID.
        /// </summary>
        /// <typeparam name="TVm">The type of the view model.</typeparam>
        /// <typeparam name="TV">The type of the layout content view.</typeparam>
        /// <param name="viewModel">The view model to use for the view.</param>
        /// <param name="layoutId">The ID of the layout to show the view in.</param>
        /// <exception cref="InvalidOperationException">Thrown if the layout with the given layout ID doesn't exist.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<TVm, TV>([NotNull]TVm viewModel, in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.Show<TVm, TV>(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView<TV>([NotNull] BaseLayoutContentViewModel viewModel, in UILayoutId layoutId) where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.ShowContent(viewModel, typeof(TV));
        }

        /// <summary>
        /// Shows a view with the given view model and layout ID.
        /// </summary>
        /// <param name="viewModel">The view model associated with the view to be displayed.</param>
        /// <param name="viewType">The type of the view to be displayed.</param>
        /// <param name="layoutId">The layout ID of the layout where the view should be displayed.</param>
        /// <exception cref="InvalidOperationException">Thrown when the layout with the specified layout ID doesn't exist.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView([NotNull]BaseLayoutContentViewModel viewModel, [NotNull] Type viewType, in UILayoutId layoutId)
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.ShowContent(viewModel, viewType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowView([NotNull]BaseLayoutContentViewModel viewModel, in UILayoutId layoutId)
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.ShowContent(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency([NotNull]BaseLayoutContentViewModel viewModel, [NotNull]Type ownerType)
        {
            var layout = m_InternalManager.FindLayoutWithContentType(ownerType);
            if (!layout.IsValid)
            {
                Debug.LogError($"View with view model type {ownerType} not found.");
                return;
            }
            
            if(!layout.ViewModel.TryGet(ownerType, out var ownerViewModel))
                return;
            
            layout.ShowContent(viewModel);
            ownerViewModel.AddDependency(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency([NotNull] BaseLayoutContentViewModel viewModel, [NotNull] Type viewType, [NotNull]Type ownerType)
        {
            var layout = m_InternalManager.FindLayoutWithContentType(ownerType);
            if (!layout.IsValid)
            {
                Debug.LogError($"View with view model type {ownerType} not found.");
                return;
            }
            
            if(!layout.ViewModel.TryGet(ownerType, out var ownerViewModel))
                return;
            
            layout.ShowContent(viewModel, viewType);
            ownerViewModel.AddDependency(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency<TV>([NotNull] BaseLayoutContentViewModel viewModel, [NotNull]Type ownerType) where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayoutWithContentType(ownerType);
            if (!layout.IsValid)
            {
                Debug.LogError($"View with view model type {ownerType} not found.");
                return;
            }
            
            if(!layout.ViewModel.TryGet(ownerType, out var ownerViewModel))
                return;
            
            layout.ShowContent(viewModel, typeof(TV));
            ownerViewModel.AddDependency(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency<TVm, TV>([NotNull]TVm viewModel, [NotNull]Type ownerType) 
            where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayoutWithContentType(ownerType);
            if (!layout.IsValid)
            {
                Debug.LogError($"View with view model type {ownerType} not found.");
                return;
            }
            
            if(!layout.ViewModel.TryGet(ownerType, out var ownerViewModel))
                return;
            
            layout.ShowContent(viewModel, typeof(TV));
            ownerViewModel.AddDependency(viewModel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency<TOwner>([NotNull]BaseLayoutContentViewModel viewModel) where TOwner : BaseLayoutContentViewModel
        {
            ShowViewAsDependency(viewModel, typeof(TOwner));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency<TV, TOwner>([NotNull] BaseLayoutContentViewModel viewModel) where TV : LayoutContentView where TOwner : BaseLayoutContentViewModel
        {
            ShowViewAsDependency<TV>(viewModel, typeof(TOwner));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowViewAsDependency<TVm, TV, TOwner>([NotNull]TVm viewModel) 
            where TVm : BaseLayoutContentViewModel where TV : LayoutContentView where TOwner : BaseLayoutContentViewModel
        {
            ShowViewAsDependency<TVm, TV>(viewModel, typeof(TOwner));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HideView<T>([NotNull] T viewModel, in UILayoutId layoutId, bool unregisterTemplate = false) where T : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            layout.HideContent(viewModel, unregisterTemplate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView<T>(in UILayoutId layoutId, bool unregisterTemplate = false) where T : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            
            return layout.HideContent(typeof(T), unregisterTemplate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView([NotNull] Type contentType, in UILayoutId layoutId, bool unregisterTemplate = false)
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
            return layout.HideContent(contentType, unregisterTemplate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView([NotNull] Type contentType, bool unregisterTemplate = false)
        {
            var layout = m_InternalManager.FindLayoutWithContentType(contentType);
            if (!layout.IsValid)
            {
                Debug.LogError($"View with view model type {contentType} not found.");
                return false;
            }

            return layout.HideContent(contentType, unregisterTemplate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HideView<T>(bool unregisterTemplate = false)
            where T : BaseLayoutContentViewModel
        {
            return HideView(typeof(T), unregisterTemplate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasView([NotNull]Type contentType)
        {
            return m_InternalManager.FindLayoutWithContentType(contentType).IsValid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasView<T>() where T : BaseLayoutContentViewModel
        {
            return m_InternalManager.FindLayoutWithContentType(typeof(T)).IsValid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterView<TVm, TV>(in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.RegisterView<TVm, TV>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterView<TVm>(in UILayoutId layoutId) where TVm : BaseLayoutContentViewModel
        {
            var layout = m_InternalManager.FindLayout(layoutId);
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            if(!layout.IsValid)
                throw new InvalidOperationException($"Layout with Id {layoutId} doesn't exist.");
#endif
                
            layout.UnregisterView<TVm>();
        }
    }
}