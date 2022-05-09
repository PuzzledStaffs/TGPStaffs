using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.DungeonEditor;
public class GenerateStructure : MonoBehaviour
{
    [SerializeField] DungeonPLayout ScriptableObject;

    public List<GameObject[,]> ListOfGameObjectArray;

    private void Start()
    {
        if (ScriptableObject == null)
        {
            Debug.Log("Missing Dungeon Image");
            return;
        }

        //foreach (DungeonPLayer layer in ScriptableObject.layers)
        //{


        //    GameObject[,] Array2D = new GameObject[layer.tex.width, layer.tex.height];

        //    for (int x = 0; x < layer.tex.width; x++)
        //    {
        //        for (int z = 0; z < layer.tex.height; z++)
        //        {
        //            Color XZColor = layer.tex.GetPixel(x, z);
        //            if (layer.ParentLayout.KeyData.ContainsKey(XZColor))
        //            {
        //                GameObject Object;
        //                if(layer.ParentLayout.KeyData.TryGetValue(XZColor, out Object))
        //                {
        //                    Array2D[x, z] = Object;
        //                }
        //                else
        //                {
        //                    Array2D[x, z] = null;
        //                    Debug.Log($"{x}, {z} is null");
        //                }
        //            }
        //            else
        //            {
        //                Array2D[x, z] = null;
        //            }
        //        }
        //    }
        //}

        GameObject[,] Array2D = new GameObject[ScriptableObject.FinalLayout.width, ScriptableObject.FinalLayout.height];

        for (int x = 0; x < ScriptableObject.FinalLayout.width; x++)
        {
            for (int z = 0; z < ScriptableObject.FinalLayout.height; z++)
            {
                Color XZColor = ScriptableObject.FinalLayout.GetPixel(x, z);
                if (ScriptableObject.KeyData.ContainsKey(XZColor))
                {
                    GameObject Object;
                    if (ScriptableObject.KeyData.TryGetValue(XZColor, out Object))
                    {
                        Array2D[x, z] = Object;
                    }
                    else
                    {
                        Array2D[x, z] = null;
                        Debug.Log($"{x}, {z} is null");
                    }
                }
                else
                {
                    Array2D[x, z] = null;
                }
            }
        }




    }
}
