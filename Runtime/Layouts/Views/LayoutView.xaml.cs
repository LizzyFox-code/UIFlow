#if UNITY_5_3_OR_NEWER
    #define NOESIS
    using Noesis;
#else
    using System.Windows;
    using System.Windows.Controls;
#endif

namespace UIFlow.Runtime.Layouts.Views
{
#if NOESIS
    using ViewModels;
#endif

    [TemplatePart(Name = "PART_FirstPresenter", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_SecondPresenter", Type = typeof(ContentControl))]
    public partial class LayoutView : UserControl
    {
        private ContentControl m_FirstPresenter;
        private ContentControl m_SecondPresenter;
        
        public LayoutView()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            NoesisUnity.LoadComponent(this, "Packages/com.lizzyfox-code.noesis-ui-flow/Runtime/Layouts/Views/LayoutView.xaml");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        
            m_FirstPresenter = FindName("PART_FirstPresenter") as ContentControl;
            m_SecondPresenter = FindName("PART_SecondPresenter") as ContentControl;
            
            SubscribeToViewModel();
        }

        private void SubscribeToViewModel()
        {
            if(DataContext == null)
                return;

            if (!(DataContext is BaseLayoutViewModel layoutViewModel))
                return;
            
            layoutViewModel.LayoutChanged += OnLayoutChanged;
        }

        private void OnLayoutChanged(ILayoutViewModel layoutViewModel, BaseLayoutContentViewModel oldContent, BaseLayoutContentViewModel newContent)
        {
            var currentContentControl = m_FirstPresenter.HasContent ? m_FirstPresenter : m_SecondPresenter;
            if (newContent == null)
            {
                HideContentWithAnimation(currentContentControl);
                return;
            }

            var nextContentControl = m_FirstPresenter.HasContent ? m_SecondPresenter : m_FirstPresenter;
            ShowContentWithAnimation(nextContentControl, newContent);
            HideContentWithAnimation(currentContentControl);
        }

        private void ShowContentWithAnimation(ContentControl contentControl, BaseLayoutContentViewModel contentViewModel)
        {
            Panel.SetZIndex(contentControl, 1);
            
            contentViewModel.Showed += model =>
            {
                contentControl.Visibility = Visibility.Visible;
                
                var content = contentControl.FindVisualChild<LayoutContentView>();
                if(content == null)
                    return;

                PlayShowAnimation(content.ShowStoryboard, content, contentControl);
            };
            contentControl.Content = contentViewModel;
        }

        private void HideContentWithAnimation(ContentControl contentControl)
        {
            contentControl.IsHitTestVisible = false;
            Panel.SetZIndex(contentControl, 0);
            
            if (!contentControl.HasContent)
                return;
            
            var content = contentControl.FindVisualChild<LayoutContentView>();
            if (content == null || content.HideStoryboard == null)
            {
                contentControl.Content = null;
                contentControl.Visibility = Visibility.Hidden;
                return;
            }

            PlayHideAnimation(content.HideStoryboard, content, contentControl);
        }

        private void PlayShowAnimation(Storyboard storyboard, FrameworkElement target, ContentControl contentControl)
        {
            if (storyboard == null)
            {
                contentControl.IsHitTestVisible = true;
                return;
            }
            
            storyboard.Completed += (sender, args) =>
            {
                contentControl.IsHitTestVisible = true;
            };
            
            storyboard.Begin(target);
        }

        private void PlayHideAnimation(Storyboard storyboard, FrameworkElement target, ContentControl contentControl)
        {
            if(storyboard == null)
                return;
            
            storyboard.Completed += (sender, args) =>
            {
                contentControl.Content = null;
                contentControl.Visibility = Visibility.Hidden;
            };
            storyboard.Begin(target);
        }
#endif
    }
}