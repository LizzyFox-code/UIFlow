#if UNITY_5_3_OR_NEWER
    #define NOESIS
    using Noesis;
#else
    using System.Windows;
    using System.Windows.Controls;
#endif

namespace UIFlow.Runtime.Layouts
{
#if UNITY_5_3_OR_NEWER
    using UnityEngine.Scripting;
    using UnityEngine;
    using ViewModels;
    
    [Preserve]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public sealed class LayoutContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
#if UNITY_5_3_OR_NEWER
            if (item == null || !Application.isPlaying)
                return null;
            
            var dataContext = container.GetParentDataContext<ILayoutViewModel>();
            return dataContext?.FindTemplate(item.GetType());
#else
            return null;
#endif
        }
    }
}