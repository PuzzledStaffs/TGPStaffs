using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonInstance))]
public class DungeonCreationInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonInstance instance = (DungeonInstance)target;
        if (GUILayout.Button("Populate Dungeon Values"))
        {
            instance.GenerateDungeonValues();
        }
    }
}
