namespace Testing
{
    using System.Collections;
    using System.Collections.Generic;
    using UIFlow.Runtime;
    using UIFlow.Runtime.Layouts;
    using UnityEngine;

    public sealed class HelloWorldComponent : MonoBehaviour
    {
        [SerializeField]
        private NoesisXaml[] m_Xamls;
        
        private readonly List<object> m_LoadedXamls = new List<object>();

        private void Start()
        {
            StartCoroutine(WaitRoutine()); // need wait one frame before show any view
        }
        
        private void Test()
        {
            // preload xaml (or use absolute paths in InitializeComponent() method for each view)
            foreach (var xaml in m_Xamls)
            {
                m_LoadedXamls.Add(xaml.Load());
            }
            
            var viewModel = new HudViewModel(); // or another view model, a.o. MonoBehaviour components
            UIFlowUtility.ShowView<HudViewModel, HudView>(viewModel, UILayout.HUD); // or another UILayout
        }

        private IEnumerator WaitRoutine()
        {
            yield return new WaitForEndOfFrame();
            Test();
        }

        private void OnDestroy()
        {
            UIFlowUtility.HideView<HudViewModel>(UILayout.HUD);
            m_LoadedXamls.Clear();
        }
    }
}