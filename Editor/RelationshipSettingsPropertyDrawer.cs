namespace UIFlow.Editor
{
    using System.Linq;
    using Runtime;
    using Runtime.Layouts;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CustomPropertyDrawer(typeof(RelationshipSettings))]
    public sealed class RelationshipSettingsPropertyDrawer : PropertyDrawer
    {
        private UIFlowSettingsAsset m_SettingsAsset;

        private SerializedProperty m_RowsProperty;

        private readonly UILayoutMatrixGUI m_UILayoutMatrixGUI;
        
        private bool m_FoldoutValue = true;

        public RelationshipSettingsPropertyDrawer()
        {
            m_UILayoutMatrixGUI = new UILayoutMatrixGUI();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var container = new IMGUIContainer(() =>
            {
                OnGUIHandler(root.localBound, property, new GUIContent(property.displayName));
            });
            root.Add(container);
            return root;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawControl(position, property, label, true);
        }

        private void DrawControl(Rect position, SerializedProperty property, GUIContent label, bool renderFoldout)
        {
            Initialize();
            
            m_RowsProperty = property.FindPropertyRelative("m_Matrix");
            
            property.serializedObject.Update();

            EditorGUI.BeginProperty(position, label, property);
            if(renderFoldout)
            {
                m_FoldoutValue = EditorGUI.BeginFoldoutHeaderGroup(position, m_FoldoutValue, label);
                if (m_FoldoutValue)
                {
                    GUI.enabled = !Application.isPlaying;
                    m_UILayoutMatrixGUI.Draw(OnGetValue, OnSetValue, m_SettingsAsset.LayoutCount, m_SettingsAsset.DefaultCount);
                }
            }
            else
            {
                GUI.enabled = !Application.isPlaying;
                m_UILayoutMatrixGUI.Draw(OnGetValue, OnSetValue, m_SettingsAsset.LayoutCount, m_SettingsAsset.DefaultCount);
            }
            GUI.enabled = true;
            
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUI.EndProperty();
            
            property.serializedObject.ApplyModifiedProperties();
        }

        private void OnGUIHandler(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawControl(position, property, label, false);
        }

        private void OnSetValue(int a, int b, bool value)
        {
            var row = m_RowsProperty.GetArrayElementAtIndex(b);
            var rowProperty = row.FindPropertyRelative("m_Row");
            rowProperty.GetArrayElementAtIndex(a).boolValue = value;
        }

        private bool OnGetValue(int a, int b)
        {
            var row = m_RowsProperty.GetArrayElementAtIndex(b);
            var rowProperty = row.FindPropertyRelative("m_Row");
            return rowProperty.GetArrayElementAtIndex(a).boolValue;
        }

        private void Initialize()
        {
            if(m_SettingsAsset != null)
                return;

            m_SettingsAsset = UIFlowSettingsAsset.GetAsset();
        }
    }
}