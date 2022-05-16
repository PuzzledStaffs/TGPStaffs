using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGP.DungeonEditor;
using TGP.DungeonEditor.Snapping;
using TGP.Utilites;
using UnityEngine;


public struct Tile
{
    public GameObject Center;
    public bool Visited;
    public int X, Y;

    public Tile(GameObject obj, int x, int y)
    {
        Center = obj;
        Visited = false;
        X = x;
        Y = y;

    }
}

public class GenerateRoom : MonoBehaviour
{
    [SerializeField] DungeonPRoom ScriptableObject;

    private GameObject[,] Current2DArray;
    private Tile[,] TileArray;
    int X = 0;
    int Z = 0;
    [SerializeField] private float FillDelay = 0.2f;
    
    
    private void Start()
    {
        
        
        if (ScriptableObject == null)
        {
            Debug.Log("Missing Dungeon Image");
            return;
        }

        GameObject previousObject;
        Debug.Log(ScriptableObject.ObjectsInRoom.Count);
        foreach (SerializableMultiDimensionalArray<GameObject> layer in ScriptableObject.ObjectsInRoom) //TODO FIX THIS NO LONGER GAMEOBJECT SOME MULTIDIMENSIONAL ARRAY
        {
            Current2DArray = layer.Unserializable;
            #region TileArray
            TileArray = new Tile[layer.Unserializable.GetLength(0), layer.Unserializable.GetLength(1)];
            for (int x = 0; x < TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < TileArray.GetLength(1); y++)
                {
                    GameObject CurrentOBJ = null;
                    if (Current2DArray[x,y] != null)
                    {
                         CurrentOBJ = Instantiate(Current2DArray[x, y], new Vector3(x, 0, y),
                            Quaternion.identity);
                         X = x;
                         Z = y;
                    }
                    TileArray[x, y] = new Tile(CurrentOBJ, x, y);
                }
            }
            #endregion
            // for (int x = 0; x < TileArray.GetLength(0); x++)
            // {
            //     for (int z = 0; z < TileArray.GetLength(1); z++)
            //     {
            //         // if (layer.Unserializable[x,z] == null)
            //         //     continue;
            //         // GameObject CurrentGameObject = GameObject.Instantiate(layer.Unserializable[x, z], new Vector3(x, 0, z), Quaternion.identity);
            //         // if (CurrentGameObject.TryGetComponent(typeof(SnappableRoom),out _))
            //         // {
            //         //     //floodfill
            //         //     X = x;
            //         //     Z = z;
            //         // }
            //         
            //         
            //     }
            //     
            //     
            // }

            //ListOfGameObjectArray.Add(Array2D);
            FloodFill(TileArray[X,Z]);
        }
    }

    private void Update()
    {
        
    }

    private void FloodFill(Tile Object)
    {
        Stack<Tile> Q = new Stack<Tile>();
        Q.Push(Object);

        while (Q.Count != 0)
        {
            Tile n = Q.Pop();
            if (!n.Visited || n.Center != null)
            {
                if (n.X > 0 || n.Y > 0 || n.X <= TileArray.GetLength(0) || n.Y <= TileArray.GetLength(1) || TileArray[n.X,n.Y].Center.TryGetComponent<SnappableRoom>(out _))
                {
                    Tile CurrentTile = TileArray[n.X, n.Y];
                    if (TileArray[n.X, n.Y].Center != null && TileArray[n.X,n.Y].Visited == false)
                    {
                        var points = CurrentTile.Center.GetComponentsInChildren<SnapPoint>();
                        
                        foreach (var snappoint in points)
                        {
                            if (snappoint.SnapTarget != null)
                            {
                                continue;
                            }
                            
                            switch (snappoint.Direction)
                            {
                                case SnapPointDirection.EAST:
                                    snappoint.SnapTarget = GetNextSnappoint(snappoint, TileArray[n.X + 1, n.Y].Center);
                                    break;
                                case SnapPointDirection.WEST:
                                    snappoint.SnapTarget = GetNextSnappoint(snappoint, TileArray[n.X - 1, n.Y].Center);
                                    break;
                                case SnapPointDirection.NORTH:
                                    snappoint.SnapTarget = GetNextSnappoint(snappoint, TileArray[n.X, n.Y + 1].Center);
                                    break;
                                case SnapPointDirection.SOUTH:
                                    snappoint.SnapTarget = GetNextSnappoint(snappoint, TileArray[n.X, n.Y - 1].Center);
                                    break;
                            }
                            snappoint.JustSnap();
                            
                            
                        }

                      

                    }
                    TileArray[n.X, n.Y].Visited = true;

                    try
                    {
                        if (TileArray[n.X + 1,n.Y].Visited == false)
                        {
                            Q.Push(TileArray[n.X + 1,n.Y]);
                        }

                        if (TileArray[n.X - 1, n.Y].Visited == false)
                        {
                            Q.Push(TileArray[n.X - 1, n.Y]);
                        }

                        if (TileArray[n.X, n.Y + 1].Visited == false)
                        {
                            Q.Push(TileArray[n.X, n.Y + 1]);
                        }

                        if (TileArray[n.X, n.Y - 1].Visited == false)
                        {
                            Q.Push(TileArray[n.X, n.Y - 1]);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"{n.X}, {n.Y}");
                        Debug.Log(e);
                        //throw;
                    }
                  
                }   
            }
        }

        
    }
    
    private void FloodFill(int x, int z)
    {
        if (Current2DArray[x, z] == null)
        {
            return;
        }
        if (x < 0 || z < 0 || x >= Current2DArray.GetLength(0) || z >= Current2DArray.GetLength(1) || !Current2DArray[x,z].TryGetComponent<SnappableRoom>(out _))
        {
            return;
        }

        var points = Current2DArray[x, z].GetComponents<SnapPoint>();

        foreach (var snappoint in points)
        {
            switch (snappoint.Direction)
            {
                case SnapPointDirection.EAST:
                    snappoint.SnapTarget = GetNextSnappoint(snappoint, Current2DArray[x + 1, z]);
                    break;
                case SnapPointDirection.WEST:
                    snappoint.SnapTarget = GetNextSnappoint(snappoint, Current2DArray[x - 1, z]);
                    break;
                case SnapPointDirection.NORTH:
                    snappoint.SnapTarget = GetNextSnappoint(snappoint, Current2DArray[x, z + 1]);
                    break;
                case SnapPointDirection.SOUTH:
                    snappoint.SnapTarget = GetNextSnappoint(snappoint, Current2DArray[x, z - 1]);
                    break;
            }
        }
        
        Debug.Log($"GameObject {x} , {z} Begin");
        
        FloodFill(x + 1, z);
        FloodFill(x - 1, z);
        FloodFill(x, z + 1);
        FloodFill(x, z - 1);
        
        Debug.Log($"GameObject {x} , {z} End");
        

    }

    private SnapPoint GetNextSnappoint(SnapPoint point, GameObject nextobject)
    {
        if (nextobject == null)
        {
            return null;
        }

        if (nextobject.TryGetComponent<SnappableRoom>(out _))
        {
            var points = nextobject.GetComponentsInChildren<SnapPoint>();

            foreach (var snapPoint in points)
            {
                if (snapPoint.SnapTarget == null)
                {
                    switch (point.Direction)
                    {
                        case SnapPointDirection.EAST:
                            if (snapPoint.Direction == SnapPointDirection.WEST)
                            {
                                return snapPoint;
                            }
                            break;
                        case SnapPointDirection.WEST:
                            if (snapPoint.Direction == SnapPointDirection.EAST)
                            {
                                return snapPoint;
                            }
                            break;
                            ;
                        case SnapPointDirection.NORTH:
                            if (snapPoint.Direction == SnapPointDirection.SOUTH)
                            {
                                return snapPoint;
                            } 
                            break;
                        case SnapPointDirection.SOUTH:
                            if (snapPoint.Direction == SnapPointDirection.NORTH)
                            {
                                return snapPoint;
                            }
                            break;
                    }
                }
               
            }
            
            
        }

        return null;


    }
}
