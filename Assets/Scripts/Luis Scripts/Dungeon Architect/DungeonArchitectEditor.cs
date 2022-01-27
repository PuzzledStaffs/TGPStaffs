using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonArchitect))]
public class DungeonArchitectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Build Dungeon"))
        {

        }
        if (GUILayout.Button("Destroy Dungeon"))
        {

        }
    }
}
