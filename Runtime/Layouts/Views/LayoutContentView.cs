#if UNITY_5_3_OR_NEWER
    #define NOESIS
    using Noesis;
    using UIFlow.Runtime.Layouts.ViewModels;
    using UnityEngine;
#else
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
#endif

namespace UIFlow.Runtime.Layouts.Views
{
    public abstract class LayoutContentView : UserControl
    {
        public static readonly DependencyProperty ShowStoryboardProperty = DependencyProperty.Register(nameof(ShowStoryboard), 
            typeof(Storyboard), typeof(LayoutContentView), new UIPropertyMetadata());
        public static readonly DependencyProperty HideStoryboardProperty = DependencyProperty.Register(nameof(HideStoryboard),
            typeof(Storyboard), typeof(LayoutContentView), new UIPropertyMetadata());

        public Storyboard ShowStoryboard
        {
            get => (Storyboard) GetValue(ShowStoryboardProperty);
            set => SetValue(ShowStoryboardProperty, value);
        }

        public Storyboard HideStoryboard
        {
            get => (Storyboard) GetValue(HideStoryboardProperty);
            set => SetValue(HideStoryboardProperty, value);
        }

        static LayoutContentView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutContentView), new FrameworkPropertyMetadata(typeof(LayoutContentView)));
        }

#if NOESIS
        protected LayoutContentView()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            // workaround for Unity Editor
            if (!Application.isPlaying || DataContext == null || !(DataContext is BaseLayoutContentViewModel contentViewModel))
                return;
                
            contentViewModel.ShowCommand.Execute(null);
        }
        
        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            // workaround for Unity Editor
            if (!Application.isPlaying || DataContext == null || !(DataContext is BaseLayoutContentViewModel contentViewModel))
                return;
                
            contentViewModel.HideCommand.Execute(null);
        }
#endif
    }
}