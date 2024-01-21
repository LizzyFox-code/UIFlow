#if UNITY_5_3_OR_NEWER
    #define NOESIS
    using Noesis;
#else
    using System.Windows;
    using System.Windows.Controls;
#endif

namespace UIFlow.Runtime.Views
{
    public partial class UIContainer : UserControl
    {
        public UIContainer()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            NoesisUnity.LoadComponent(this, "Packages/com.lizzyfox-code.noesis-ui-flow/Runtime/Views/UIContainer.xaml");
        }
#endif
    }
}