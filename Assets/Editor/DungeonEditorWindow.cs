using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonEditorWindow : EditorWindow
{

    [MenuItem("Window/Dungeon Editor")]
    static void Intitalize()
    {
        DungeonEditorWindow Window = (DungeonEditorWindow)EditorWindow.GetWindow(typeof(DungeonEditorWindow));
        Window.Show();
    }

}
