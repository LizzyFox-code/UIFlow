namespace Testing
{
    using UIFlow.Runtime;
    using UIFlow.Runtime.Layouts;
    using UnityEngine;

    public sealed class HelloWorldComponent : MonoBehaviour
    {
        private void Awake()
        {
            var viewModel = new HudViewModel(); // or another view model, a.o. MonoBehaviour components
            UIFlowUtility.ShowView<HudViewModel, HudView>(viewModel, UILayout.HUD); // or another UILayout
        }

        private void OnDestroy()
        {
            UIFlowUtility.HideView<HudViewModel>(UILayout.HUD);
        }
    }
}