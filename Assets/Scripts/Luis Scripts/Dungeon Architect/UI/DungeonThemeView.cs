using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DungeonThemeView : GraphView
{
    public new class UxmlFactory: UxmlFactory<DungeonThemeView, GraphView.UxmlTraits> { };

    public DungeonThemeView()
    {
        Insert(0,new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uss");
        styleSheets.Add(stylesheet);
    }

    internal void PopulateView()
    {

    }
}
