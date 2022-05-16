using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(PrefabBrush))]
public class PrefabBrushEditor : Editor
{
    public GameObject SelectedPrefab;
    public List<GameObject> PreFabs;
    private Texture2D blueTexture;
    private Texture2D greyTexture;
    float prefabListHeight;
    private int selGridInt;
    private PrefabBrush brush;


    private void OnEnable()
    {
        brush = (target as PrefabBrush);
        if (brush == null)
        {
            Debug.Log("Brush Is Null");
        }
        blueTexture = new Texture2D(64, 64);
        greyTexture = new Texture2D(64, 64);

        for (int y = 0; y < blueTexture.height; y++)
        {
            for (int x = 0; x < blueTexture.width; x++)
            {
                //rgb(0,191,255)
                // blueTexture.SetPixel(x, y, new Color(0.25f, 0.42f, 0.66f));
                blueTexture.SetPixel(x, y, new Color(0f, 0.8f, 1f));
                greyTexture.SetPixel(x, y, new Color(0.75f, 0.75f, 0.75f));
            }
        }

        PreFabs = brush.PreFabs;
        blueTexture.Apply();
        greyTexture.Apply();
        if (PreFabs == null)
        {
            PreFabs = new List<GameObject>();
        }
    }
    
    

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(brush);   
        if (GUILayout.Button("Delete Selected Prefabs"))
        {
            DeletePrefabFromList();
        }
        EditorGUILayout.Space(50f);
        EditorGUILayout.BeginVertical();
        DropAreaGUI();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(50f);
        DisplayPrefabs(PreFabs);
        
        
    }

    public void DeletePrefabFromList()
    {
        if (SelectedPrefab != null && PreFabs.Count != 0)
        {
            brush.PrefabChosen = null;
            PreFabs.Remove(PreFabs[selGridInt]);
            brush.PreFabs.Remove(PreFabs[selGridInt]);
            brush.cells[0].gameObject = null;
            selGridInt--;
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
            Repaint();
        }
    
        
    }

    private void OnDisable()
    {
        brush.PreFabs = PreFabs;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //
    // public void DisplayPrefabs(List<GameObject> prefabs)
    // {
    //     GUILayout.BeginVertical("Box");
    //     {
    //         GUILayout.BeginHorizontal();
    //         {
    //             GUILayout.BeginVertical();
    //             {
    //                 int numberOfPrefabs = prefabs.Count;
    //                 int windowWidth = (int)EditorGUIUtility.currentViewWidth;
    //                 int columns = 13;
    //                 int rows = Mathf.CeilToInt((float) numberOfPrefabs / columns);
    //                 prefabListHeight = rows * 50f;
    //
    //                 for (int x = 0; x < rows; x++)
    //                 {
    //                     GUILayout.BeginHorizontal();
    //                     {
    //                         for (int y = 0; y < columns; y++)
    //                         {
    //
    //                             int index = y + x * columns;
    //                             
    //                             Rect prefabArea = 5 + 50 * ()
    //                             GUI.Box(prefabArea, "");
    //                             Rect border = new Rect(prefabArea.x + 2, prefabArea.y + 6, prefabArea.width - 4, prefabArea.height - 4);
    //
    //                             if (prefabArea.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0)
    //                             {
    //                                 SelectedPrefab = prefabs[y];
    //                                 Repaint();
    //                             }
    //                             if (greyTexture != null)
    //                             {
    //                                 Debug.Log(index);
    //                                 if (prefabs[index] == SelectedPrefab && SelectedPrefab != null) EditorGUI.DrawPreviewTexture(border, blueTexture, null, ScaleMode.ScaleToFit, 0f);
    //                                 else EditorGUI.DrawPreviewTexture(border, greyTexture, null, ScaleMode.ScaleToFit, 0f);
    //                                 
    //                                 
    //                             }
    //                             
    //                             border.x += 2;
    //                             border.y += 2;
    //                             border.width -= 4;
    //                             border.height -= 4;
    //
    //                             if (prefabs[index] != null)
    //                             {
    //                                 Texture2D preview = AssetPreview.GetAssetPreview(prefabs[y]);
    //                                 if (preview != null)
    //                                 {
    //                                     EditorGUI.DrawPreviewTexture(border, preview, null, ScaleMode.ScaleToFit, 0f);
    //                                 }
    //                             }
    //                            
    //                         }
    //                     }
    //                     GUILayout.EndHorizontal();
    //                 }
    //             }
    //             GUILayout.EndVertical();
    //         }
    //         GUILayout.EndHorizontal();
    //     }
    //     GUILayout.EndVertical();
    // }


    public void DisplayPrefabs(List<GameObject> prefabs)
    {
        List<Texture2D> ListOfPreviewTextures = new List<Texture2D>();
        foreach (GameObject obj in prefabs)
        {
            ListOfPreviewTextures.Add(AssetPreview.GetAssetPreview(obj));
        }

        selGridInt = GUILayout.SelectionGrid(selGridInt, ListOfPreviewTextures.ToArray(), 10);
        if (PreFabs.Count != 0)
        {
            brush.PrefabChosen = prefabs[selGridInt];
            brush.cells[0].gameObject = prefabs[selGridInt];
            SelectedPrefab = prefabs[selGridInt];

        }
        
    }
    
    // public void DisplayPrefabs(List<GameObject> prefabs)
    // {
    //    
    //      int numberOfPrefabs = prefabs.Count;
    //         int windowWidth = (int)EditorGUIUtility.currentViewWidth;
    //
    //         int y;
    //         if (SelectedPrefab != null) y = 215;
    //         else y = 110;
    //
    //         for (int i = 0; i < numberOfPrefabs; i++)
    //         {
    //             var e = Event.current;
    //             GameObject go = prefabs[i];
    //
    //             int columns = Mathf.FloorToInt(windowWidth / (50 + 20) + 1);
    //             int rows = Mathf.FloorToInt(numberOfPrefabs / columns);
    //             prefabListHeight = rows * 50f;
    //             int posX = 5 + 50 * (i - (Mathf.FloorToInt(i / columns)) * columns);
    //             int posY = y + 50 * Mathf.FloorToInt(i / columns) + 10;
    //
    //             Rect r = new Rect(posX, posY, 50, 50);
    //             GUILayout.Box();
    //             Rect border = new Rect(r.x + 2, r.y + 6, r.width - 4, r.height - 4);
    //             
    //             Debug.Log(r.Contains(Event.current.mousePosition));
    //             
    //             if (r.Contains(Event.current.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
    //             {
    //                 SelectedPrefab = prefabs[i];
    //                 Repaint();
    //             }
    //
    //             if (greyTexture != null)
    //             {
    //                 if (prefabs[i] == SelectedPrefab && SelectedPrefab != null) EditorGUI.DrawPreviewTexture(border, blueTexture, null, ScaleMode.ScaleToFit, 0f);
    //                 else EditorGUI.DrawPreviewTexture(border, greyTexture, null, ScaleMode.ScaleToFit, 0f);
    //             }
    //
    //             border.x += 2;
    //             border.y += 2;
    //             border.width -= 4;
    //             border.height -= 4;
    //
    //             Texture2D preview = AssetPreview.GetAssetPreview(go);
    //
    //             if (preview != null)
    //             {
    //                 EditorGUI.DrawPreviewTexture(border, preview, null, ScaleMode.ScaleToFit, 0f);
    //             }
    //         }
    //       
    // }

    public void DropAreaGUI()
    {
        Event e = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(50.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Add Trigger");
        
        
        switch (e.rawType)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(e.mousePosition))
                {
                    return;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (e.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object Dragged_Object in DragAndDrop.objectReferences)
                    {
                        if (PrefabUtility.GetPrefabAssetType(Dragged_Object) == PrefabAssetType.NotAPrefab)
                        {
                            Debug.Log($"Not a Prefab {Dragged_Object}");
                            continue;
                        }
                        
                        PreFabs.Add(Dragged_Object as  GameObject);
                        Repaint();
                    }
                }
                break;;
        }
    }
}
