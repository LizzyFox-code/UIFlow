namespace UIFlow.Editor
{
    using System.Collections.Generic;
    using Runtime;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    internal static class UIFlowSettingsProvider
    {
        private static UIFlowSettingsAsset m_FlowSettingsAsset;
        
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Game/UI Flow", SettingsScope.Project)
            {
                label = "UI Flow",
                activateHandler = ActivateHandler,
                keywords = new HashSet<string>(new [] {"UI", "Game", "Flow", "Settings"})
            };

            return provider;
        }

        private static void ActivateHandler(string searchContext, VisualElement rootVisualElement)
        {
            if (m_FlowSettingsAsset == null)
                m_FlowSettingsAsset = UIFlowSettingsAsset.GetAsset();
                
            var serializedObject = new SerializedObject(m_FlowSettingsAsset);
            UIFlowSettingsAssetEditorInternal.CreateInspector(rootVisualElement, serializedObject);
            rootVisualElement.Bind(serializedObject);
        }
    }
}