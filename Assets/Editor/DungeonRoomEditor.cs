using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungenRoom))]
public class DungeonRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DungenRoom myTarget = (DungenRoom)target;

        DrawDefaultInspector();

        GUILayout.BeginArea(new Rect(0.0f, 0.0f, 100, 100));
        if (GUILayout.Button("Build Object"))
        {

        }
        GUILayout.EndArea();
    }
}
