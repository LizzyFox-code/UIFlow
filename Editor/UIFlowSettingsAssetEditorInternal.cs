namespace UIFlow.Editor
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal static class UIFlowSettingsAssetEditorInternal
    {
        public static void CreateInspector([NotNull]VisualElement rootVisualElement, SerializedObject serializedObject)
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UIFlow/UIFlowSettingsView");
            visualTree.CloneTree(rootVisualElement);

            var viewContainerProperty = rootVisualElement.Q<PropertyField>("view-container-property");
            viewContainerProperty.SetEnabled(false);
            var xamlContainerProperty = rootVisualElement.Q<PropertyField>("xaml-container-property");
            xamlContainerProperty.SetEnabled(false);

            var defaultLayoutsListView = rootVisualElement.Q<ListView>("default-layouts-list-view");
            defaultLayoutsListView.SetEnabled(false);
            
            var defaultLayoutsProperty = serializedObject.FindProperty("m_DefaultLayouts");
            
            defaultLayoutsListView.makeItem = OnLayoutItemViewMakeItem;
            defaultLayoutsListView.bindItem = (a, b) => OnLayoutItemViewBindItem(a, b, defaultLayoutsProperty);

            var layoutsListView = rootVisualElement.Q<ListView>("layouts-list-view");
            var layoutsProperty = serializedObject.FindProperty("m_Layouts");

            layoutsListView.makeItem = OnLayoutItemViewMakeItem;
            layoutsListView.bindItem = (a, b) => OnLayoutItemViewBindItem(a, b, layoutsProperty);
        }
        
        private static VisualElement OnLayoutItemViewMakeItem()
        {
            var root = new BindableElement();
            var visualTree = Resources.Load<VisualTreeAsset>("UIFlow/LayoutItemView");
            visualTree.CloneTree(root);
            return root;
        }
        
        private static void OnLayoutItemViewBindItem(VisualElement element, int index, SerializedProperty listProperty)
        {
            var elementProperty = listProperty.GetArrayElementAtIndex(index);
            var viewModelProperty = elementProperty.FindPropertyRelative("m_ViewModel");
            var selector = element.Q<LayoutViewModelSelector>();
            selector.Property = viewModelProperty;
            
            (element as BindableElement).BindProperty(elementProperty);
        }
    }
}