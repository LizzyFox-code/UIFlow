namespace UIFlow.Runtime
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Noesis;
    
    public static class DataTemplateUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataTemplate CreateTemplate<TVm, TV>() where TVm : class where TV : FrameworkElement
        {
            return CreateTemplate(typeof(TVm), typeof(TV));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataTemplate CreateTemplate([NotNull]Type viewModelType, [NotNull]Type viewType)
        {
            const string xamlTemplate = "<DataTemplate\n xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\n xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n xmlns:vm=\"clr-namespace:{2};assembly={4}\"\n xmlns:v=\"clr-namespace:{3};assembly={5}\"\n DataType=\"{{x:Type vm:{0}}}\">\n <v:{1} />\n </DataTemplate>";

            var xaml = string.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace,
                viewType.Namespace, viewModelType.Assembly.GetName().Name, viewType.Assembly.GetName().Name);

            return (DataTemplate) XamlReader.Parse(xaml);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterDataTemplate<TVm, TV>([NotNull]FrameworkElement target) where TVm : class where TV : FrameworkElement
        {
            RegisterDataTemplate(target, typeof(TVm), typeof(TV));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterDataTemplate([NotNull]FrameworkElement target, [NotNull]Type viewModelType, [NotNull]Type viewType)
        {
            var dataTemplate = CreateTemplate(viewModelType, viewType);
            target.Resources.Add(dataTemplate.DataType, dataTemplate);
        }
    }
}