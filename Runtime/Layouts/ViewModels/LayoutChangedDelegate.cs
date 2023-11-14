namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    public delegate void LayoutChangedDelegate([NotNull]ILayoutViewModel layoutViewModel, BaseLayoutContentViewModel content);
}