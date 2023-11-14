namespace UIFlow.Editor
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Runtime.Layouts.ViewModels;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal sealed class LayoutViewModelSelector : BindableElement
    {
        private static readonly Regex m_TypeNameRegex = new Regex(@"^managedReference<(\w*)>");
        private static readonly Regex m_CapitalLettersRegex = new Regex(@"(.)([A-Z])");

        private static readonly string m_NullTypeName = $"{nameof(ILayoutViewModel)}: [Null]";
        private static readonly Type m_DerivedType = typeof(ILayoutViewModel);
    
        private GenericDropdownMenu m_SelectDropdownMenu;
        private readonly VisualElement m_Container;
        private readonly Button m_SelectButton;
        private readonly Label m_NameLabel;
        
        // workaround
        private SerializedProperty m_SerializedProperty;

        public SerializedProperty Property
        {
            get => m_SerializedProperty;
            set
            {
                if (m_SerializedProperty == value)
                    return;
                
                m_SerializedProperty = value;
                Rebind();
            }
        }
        
        public LayoutViewModelSelector()
        {
            AddToClassList("layout-view-model-selector");
            
            var visualTree = Resources.Load<VisualTreeAsset>("UIFlow/LayoutViewModelSelectorView");
            visualTree.CloneTree(this);
            styleSheets.Add(Resources.Load<StyleSheet>("UIFlow/LayoutViewModelSelectorStyle"));
            
            m_SelectButton = this.Q<Button>("select-button");
            m_SelectButton.RegisterCallback<ClickEvent>(OnSelectButtonClick);

            m_Container = this.Q<VisualElement>("container");
            m_NameLabel = this.Q<Label>("name-label");
        }

        public void Rebind()
        {
            m_Container.Clear();
            if(m_SerializedProperty == null)
                return;

            var goodLookedTypeName = m_SerializedProperty.boxedValue != null
                ? GetGoodLookedTypeName(m_SerializedProperty.type)
                : m_NullTypeName;
            
            m_NameLabel.text = goodLookedTypeName;
            
            var propertyField = new PropertyField();
            propertyField.BindProperty(m_SerializedProperty);
            m_Container.Add(propertyField);
        }
        
        private void OnSelectButtonClick(ClickEvent evt)
        {
            m_SelectDropdownMenu = new GenericDropdownMenu();
            FillAddDropdownMenu(m_SelectDropdownMenu);
            
            m_SelectDropdownMenu.DropDown(m_SelectButton.worldBound, m_SelectButton);
        }
        
        private void FillAddDropdownMenu(GenericDropdownMenu dropdownMenu)
        {
            var types = TypeCache.GetTypesDerivedFrom(m_DerivedType);
            foreach (var type in types)
            {
                if(type.IsAbstract)
                    continue;

                var itemName = m_CapitalLettersRegex.Replace(type.Name, "$1 $2");
                if (itemName == m_NameLabel.text || type.GetCustomAttribute<DefaultLayoutViewModelAttribute>() != null)
                {
                    dropdownMenu.AddDisabledItem(itemName, false);
                    continue;
                }
                dropdownMenu.AddItem(itemName, false, OnGenericMenuItemAction, type);
            }
            
            if (types.Count == 0)
            {
                dropdownMenu.AddDisabledItem("No items available", false);
            }
        }

        private void OnGenericMenuItemAction(object obj)
        {
            if(!(obj is Type type))
                return;

            var instance = Activator.CreateInstance(type);
            m_SerializedProperty.boxedValue = instance;
            m_SerializedProperty.serializedObject.ApplyModifiedProperties();

            Rebind();
        }

        private static string GetGoodLookedTypeName(string type)
        {
            var typeMatch = m_TypeNameRegex.Match(type);
            return m_CapitalLettersRegex.Replace(typeMatch.Groups[1].Value, "$1 $2");
        }
        
        public new class UxmlTraits : BindableElement.UxmlTraits { }
        public new class UxmlFactory : UxmlFactory<LayoutViewModelSelector, UxmlTraits> { }
    }
}