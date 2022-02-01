using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DungeonThemeEditor : EditorWindow
{

    DungeonThemeSceneView DungeonPreview;
    InspectorView inspector;
    DungeonThemeView DungeonThemeGrid;
    RuleList RL;

    [MenuItem("Dungeon/Dungeon Layout Editor")]
    public static void ShowGUI()
    {
        DungeonThemeEditor wnd = GetWindow<DungeonThemeEditor>();
        wnd.titleContent = new GUIContent("DungeonEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uxml");
        VisualTree.CloneTree(root);

        var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uss");
        root.styleSheets.Add(stylesheet);

        DungeonPreview = root.Q<DungeonThemeSceneView>();
        inspector = root.Q<InspectorView>();
        DungeonThemeGrid = root.Q<DungeonThemeView>();
        RL = root.Q<RuleList>();
        RL.SetupList();

        //DungeonThemeGrid.OnNodeSelected = OnNodeSelectionChanged;
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspector.UpdateSelection(node);
    }
}
