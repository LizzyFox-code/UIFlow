namespace UIFlow.Editor
{
    using Runtime;
    using UnityEditor;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(UIFlowSettingsAsset))]
    public sealed class UIFlowSettingsAssetEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var rootVisualElement = new VisualElement();
            UIFlowSettingsAssetEditorInternal.CreateInspector(rootVisualElement, serializedObject);
            return rootVisualElement;
        }
    }
}