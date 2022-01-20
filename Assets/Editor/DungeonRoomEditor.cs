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

        if (GUILayout.Button("DELETE Floor"))
        {
            myTarget.ResetFloor();
        }
    }
}
