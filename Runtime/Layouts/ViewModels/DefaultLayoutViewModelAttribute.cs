namespace UIFlow.Runtime.Layouts.ViewModels
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class DefaultLayoutViewModelAttribute : Attribute
    {
    }
}