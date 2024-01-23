namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Noesis;
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class UILayoutViewModelExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<TVm, TV>([NotNull]this ILayoutViewModel viewModel, [NotNull] TVm item)
            where TVm : BaseLayoutContentViewModel where TV : FrameworkElement
        {
            viewModel.Add(item, typeof(TV));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterView<TVm, TV>([NotNull]this ILayoutViewModel viewModel) where TVm : BaseLayoutContentViewModel where TV : FrameworkElement
        {
            viewModel.RegisterView(typeof(TVm), typeof(TV));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterView<TVm>([NotNull]this ILayoutViewModel viewModel) where TVm : BaseLayoutContentViewModel
        {
            viewModel.UnregisterView(typeof(TVm));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataTemplate FindTemplate<TVm>([NotNull]this ILayoutViewModel viewModel) where TVm : BaseLayoutContentViewModel
        {
            return viewModel.FindTemplate(typeof(TVm));
        }
    }
}