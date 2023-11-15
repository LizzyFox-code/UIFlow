namespace UIFlow.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Layouts;
    using Layouts.ViewModels;
    using UnityEngine;

    internal sealed class UIFlowSettingsAsset : ScriptableObject
    {
#if UNITY_EDITOR
        private static NoesisXaml m_DefaultXaml;

        private const string DefaultContainerXamlPath = "Packages/com.lizzyfox-code.noesis-ui-flow/Runtime/Views/UIContainer.xaml";
        private const string DefaultLayoutXamlPath = "Packages/com.lizzyfox-code.noesis-ui-flow/Runtime/Layouts/Views/LayoutView.xaml";
#endif

        [SerializeField]
        private NoesisView m_ViewContainer;
        [SerializeField]
        private NoesisXaml m_XamlContainer;
        [SerializeField, NonReorderable]
        private List<UILayoutData> m_DefaultLayouts = new List<UILayoutData>();
        [SerializeField, NonReorderable]
        private List<UILayoutData> m_Layouts = new List<UILayoutData>();
        
        [SerializeField]
        private RelationshipSettings m_RelationshipSettings = new RelationshipSettings();

        public NoesisView ViewContainer => m_ViewContainer;

        public NoesisXaml XamlContainer => m_XamlContainer;

        public RelationshipSettings RelationshipSettings => m_RelationshipSettings;

        internal int DefaultCount => m_DefaultLayouts.Count;
        public int LayoutCount => m_DefaultLayouts.Count + m_Layouts.Count;

        public void GetLayouts([NotNull]List<UILayoutData> layouts)
        {
            layouts.AddRange(m_DefaultLayouts);
            layouts.AddRange(m_Layouts);
        }

        public static UIFlowSettingsAsset GetAsset()
        {
#if UNITY_EDITOR
            var assetPath = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(UIFlowSettingsAsset)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath).FirstOrDefault();

            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UIFlowSettingsAsset>(assetPath);
            if (asset == null)
                asset = CreateSettingsAsset();

            return asset;
#else
            return Resources.FindObjectsOfTypeAll<UIFlowSettingsAsset>().FirstOrDefault();
#endif
        }

#if UNITY_EDITOR
        private static UIFlowSettingsAsset CreateSettingsAsset()
        {
            var filePath = $"Assets/{nameof(UIFlowSettingsAsset)}.asset";
            
            var asset = CreateInstance<UIFlowSettingsAsset>();
            asset.name = nameof(UIFlowSettingsAsset);

            SetDefault(asset);
            
            UnityEditor.AssetDatabase.CreateAsset(asset, filePath);
            return asset;
        }
        
        private void Reset()
        {
            SetDefault(this);
        }

        private static void SetDefault([NotNull]UIFlowSettingsAsset asset)
        {
            asset.m_DefaultLayouts.Clear();
            asset.m_Layouts.Clear();
            asset.m_RelationshipSettings.Clear();

            asset.m_XamlContainer = UnityEditor.AssetDatabase.LoadAssetAtPath<NoesisXaml>(DefaultContainerXamlPath);
            asset.m_ViewContainer = Resources.Load<NoesisView>("UIFlow/UI Camera");
            
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("Before HUD", 0, new BeforeHudLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("HUD", 1, new HudLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("After HUD", 2, new AfterHudLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("Popups", 10, new PopupsLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("Screens", 20, new ScreensLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("Overlay", 30, new OverlayLayoutViewModel()));
            asset.m_DefaultLayouts.Add(CreateDefaultLayoutData("Loading", 31, new LoadingLayoutViewModel()));

            var matrix = asset.m_RelationshipSettings.Matrix;
            ApplyBeforeHudMatrix(ref matrix);
            ApplyHudMatrix(ref matrix);
            ApplyAfterHudMatrix(ref matrix);
            ApplyPopupsMatrix(ref matrix);
            ApplyScreensMatrix(ref matrix);
            ApplyOverlayMatrix(ref matrix);
            ApplyLoadingMatrix(ref matrix);
            asset.m_RelationshipSettings.Matrix = matrix;
        }
        
        private static UILayoutData CreateDefaultLayoutData([NotNull]string layoutName, int priority, [NotNull]ILayoutViewModel viewModel)
        {
            if(m_DefaultXaml == null)
                m_DefaultXaml = UnityEditor.AssetDatabase.LoadAssetAtPath<NoesisXaml>(DefaultLayoutXamlPath);
            
            return new UILayoutData(layoutName, priority, m_DefaultXaml, viewModel);
        }

        private static void ApplyBeforeHudMatrix(ref bool[,] matrix)
        {
            matrix[0, 0] = true;
            matrix[0, 1] = true;
            matrix[0, 2] = true;
            matrix[0, 3] = false;
            matrix[0, 4] = false;
            matrix[0, 5] = false;
            matrix[0, 6] = false;

            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[0, i] = false;
            }
        }
        
        private static void ApplyHudMatrix(ref bool[,] matrix)
        {
            matrix[1, 0] = true;
            matrix[1, 1] = true;
            matrix[1, 2] = true;
            matrix[1, 3] = true;
            matrix[1, 4] = false;
            matrix[1, 5] = false;
            matrix[1, 6] = false;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[1, i] = false;
            }
        }
        
        private static void ApplyAfterHudMatrix(ref bool[,] matrix)
        {
            matrix[2, 0] = true;
            matrix[2, 1] = true;
            matrix[2, 2] = true;
            matrix[2, 3] = false;
            matrix[2, 4] = false;
            matrix[2, 5] = false;
            matrix[2, 6] = false;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[2, i] = false;
            }
        }
        
        private static void ApplyPopupsMatrix(ref bool[,] matrix)
        {
            matrix[3, 0] = false;
            matrix[3, 1] = true;
            matrix[3, 2] = false;
            matrix[3, 3] = true;
            matrix[3, 4] = false;
            matrix[3, 5] = false;
            matrix[3, 6] = false;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[3, i] = false;
            }
        }
        
        private static void ApplyScreensMatrix(ref bool[,] matrix)
        {
            matrix[4, 0] = false;
            matrix[4, 1] = false;
            matrix[4, 2] = false;
            matrix[4, 3] = false;
            matrix[4, 4] = true;
            matrix[4, 5] = false;
            matrix[4, 6] = false;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[4, i] = false;
            }
        }
        
        private static void ApplyOverlayMatrix(ref bool[,] matrix)
        {
            matrix[5, 0] = false;
            matrix[5, 1] = false;
            matrix[5, 2] = false;
            matrix[5, 3] = false;
            matrix[5, 4] = false;
            matrix[5, 5] = true;
            matrix[5, 6] = false;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[5, i] = false;
            }
        }
        
        private static void ApplyLoadingMatrix(ref bool[,] matrix)
        {
            matrix[6, 0] = false;
            matrix[6, 1] = false;
            matrix[6, 2] = false;
            matrix[6, 3] = false;
            matrix[6, 4] = false;
            matrix[6, 5] = false;
            matrix[6, 6] = true;
            
            for (var i = 7; i < LayoutTable.MaxLayoutCount; i++)
            {
                matrix[6, i] = false;
            }
        }
#endif
    }
}