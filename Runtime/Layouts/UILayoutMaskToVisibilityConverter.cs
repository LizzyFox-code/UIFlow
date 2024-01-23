#if UNITY_5_3_OR_NEWER
    #define NOESIS
    using Noesis;
#else
    using System.Windows;
    using System.Windows.Data;
#endif

namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Globalization;

#if UNITY_5_3_OR_NEWER
    using UnityEngine.Scripting;
    using ViewModels;
    
    [Preserve]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public sealed class UILayoutMaskToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
#if UNITY_5_3_OR_NEWER
            if(value.Length != 2)
                return Visibility.Collapsed;
            if (!(value[0] is UILayoutMask mask) || !(value[1] is FrameworkElement frameworkElement))
                return Visibility.Collapsed;
            
            if(frameworkElement.DataContext == null || !(frameworkElement.DataContext is ILayoutViewModel layoutViewModel))
                return Visibility.Collapsed;

            var shouldBeVisible = mask.GetValue(layoutViewModel.Id);
            return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
#else
            return Visibility.Visible;
#endif
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}