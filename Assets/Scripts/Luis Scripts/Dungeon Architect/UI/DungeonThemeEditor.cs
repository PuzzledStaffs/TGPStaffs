using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class DungeonThemeEditor : EditorWindow
{
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uxml");
        VisualTree.CloneTree(root);

        var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uss");
        root.styleSheets.Add(stylesheet);
    }
}
