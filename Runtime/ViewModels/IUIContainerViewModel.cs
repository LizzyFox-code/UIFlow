namespace UIFlow.Runtime.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Layouts.ViewModels;

    internal interface IUIContainerViewModel
    {
        void AddLayout([NotNull] ILayoutViewModel viewModel, int priority);

        void RemoveLayout([NotNull] ILayoutViewModel viewModel);
    }
}