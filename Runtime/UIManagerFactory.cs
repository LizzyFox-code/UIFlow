namespace UIFlow.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Layouts;
    using UnityEngine;
    using UnityEngine.Pool;
    using ViewModels;

    internal sealed class UIManagerFactory
    {
        public UIManager Create([NotNull] UIFlowSettingsAsset settingsAsset)
        {
            var noesisView = CreateViewContainer(settingsAsset);

            var layoutTable = new LayoutTable();
            var relationshipTable = new LayoutRelationshipTable();
            
            var viewModel = new UIContainerViewModel(layoutTable, relationshipTable);
            var uiManager = UIFlowUtility.m_InternalManager = new UIManager(noesisView, viewModel, layoutTable, relationshipTable);

            var layouts = ListPool<UILayoutData>.Get();
            settingsAsset.GetLayouts(layouts);
            LoadLayouts(uiManager, layouts, settingsAsset.RelationshipSettings);
            ListPool<UILayoutData>.Release(layouts);

            return uiManager;
        }
        
        private NoesisView CreateViewContainer([NotNull] UIFlowSettingsAsset settingsAsset)
        {
            var xaml = LoadXamlContainer(settingsAsset);
            var view = settingsAsset.ViewContainer;
            
            var instance = Object.Instantiate(view, Vector3.zero, Quaternion.identity);
            Object.DontDestroyOnLoad(instance.gameObject);

            instance.Xaml = xaml;
            instance.LoadXaml(true);

            return instance;
        }

        private NoesisXaml LoadXamlContainer([NotNull]UIFlowSettingsAsset settingsAsset)
        {
            var xaml = settingsAsset.XamlContainer;
            return Object.Instantiate(xaml);
        }
        
        private static void LoadLayouts([NotNull]UIManager uiManager, [NotNull]IReadOnlyList<UILayoutData> layouts, [NotNull]RelationshipSettings relationshipSettings)
        {
            for (var i = 0; i < layouts.Count; i++)
            {
                var layoutData = layouts[i];
                if(layoutData.Xaml == null || layoutData.ViewModel == null)
                    continue;
                
                // preload layout view type
                var xaml = layoutData.Xaml.Load();
                var mask = relationshipSettings.GetMask(i);
                uiManager.RegisterLayout(xaml, layoutData.ViewModel, layoutData.Priority, mask, layoutData.Name);
            }
        }
    }
}