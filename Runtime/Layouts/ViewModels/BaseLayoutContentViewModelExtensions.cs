namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Views;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class BaseLayoutContentViewModelExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Show<TV>(this BaseLayoutContentViewModel viewModel, in UILayoutId layoutId) where TV : LayoutContentView
        {
            UIFlowUtility.ShowView(viewModel, typeof(TV), layoutId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Hide(this BaseLayoutContentViewModel viewModel, bool unregisterTemplate = false)
        {
            UIFlowUtility.HideView(viewModel.GetType(), unregisterTemplate);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowDependency(this BaseLayoutContentViewModel owner, [NotNull]BaseLayoutContentViewModel viewModel)
        {
            UIFlowUtility.ShowViewAsDependency(viewModel, owner.GetType());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowDependency(this BaseLayoutContentViewModel owner, [NotNull] BaseLayoutContentViewModel viewModel, [NotNull] Type viewType)
        {
            UIFlowUtility.ShowViewAsDependency(viewModel, viewType, owner.GetType());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowDependency<TV>(this BaseLayoutContentViewModel owner, [NotNull] BaseLayoutContentViewModel viewModel) where TV : LayoutContentView
        {
            UIFlowUtility.ShowViewAsDependency(viewModel, typeof(TV), owner.GetType());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowDependency<TVm, TV>(this BaseLayoutContentViewModel owner, [NotNull]TVm viewModel) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            UIFlowUtility.ShowViewAsDependency(viewModel, typeof(TV), owner.GetType());
        }
    }
}