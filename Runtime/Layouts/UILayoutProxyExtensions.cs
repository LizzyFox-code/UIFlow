namespace UIFlow.Runtime.Layouts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using ViewModels;
    using Views;

    public static class UILayoutProxyExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Show<TVm, TV>([NotNull]this UILayoutProxy layoutProxy, [NotNull]TVm item) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            layoutProxy.ShowContent(item, typeof(TV));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterView<TVm, TV>([NotNull]this UILayoutProxy layoutProxy) where TVm : BaseLayoutContentViewModel where TV : LayoutContentView
        {
            layoutProxy.RegisterContentViewType(typeof(TVm), typeof(TV));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterView<TVm>([NotNull]this UILayoutProxy layoutProxy) where TVm : BaseLayoutContentViewModel
        {
            layoutProxy.UnregisterContentViewType(typeof(TVm));
        }
    }
}