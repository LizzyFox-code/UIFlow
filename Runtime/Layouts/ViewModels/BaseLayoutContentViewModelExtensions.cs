namespace UIFlow.Runtime.Layouts.ViewModels
{
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
    }
}