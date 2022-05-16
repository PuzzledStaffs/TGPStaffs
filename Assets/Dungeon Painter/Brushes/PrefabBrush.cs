using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false,true,false,"Prefab Brush")]
public class PrefabBrush : GameObjectBrush
{
    public GameObject PrefabChosen;

    public List<GameObject> PreFabs = new List<GameObject>();
    
    
    
    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget.layer == 31)
        {
            return;
            ;
        }
        
        Transform erased =
            GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, 0));
        if (erased != null)
        {
            Undo.DestroyObjectImmediate(erased.gameObject);
        }
    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));

        Bounds bounds = new Bounds((max + min) * 0.5f, max - min);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position))
            {
                return child;
            }
        }
        
        return null;
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    { 
        base.Paint(gridLayout, brushTarget, position);

        Transform ToMove =
            GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, 0));

        ToMove.position = new Vector3(ToMove.position.x, brushTarget.transform.position.y, ToMove.position.z);

    }
}
