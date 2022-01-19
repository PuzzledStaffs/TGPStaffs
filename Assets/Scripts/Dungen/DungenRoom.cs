using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenRoom : MonoBehaviour
{
    [SerializeField] private DungonCamaraControler m_Camera;
    [SerializeField] private RoomType m_RoomType;
    [SerializeField] private GameObject m_origin;
    [SerializeField] private bool m_PlayerStartingRoom = false;
    public List<DungenDoor> m_doorsIn;
    public List<DungenDoor> m_doorsOut;
    public List<GameObject> m_Enemies;
    [SerializeField] List<Trap> m_traps;

    private void Start()
    {
        if (m_PlayerStartingRoom)
        {
            RoomEntered();
            m_Camera.transform.position = m_origin.transform.position;
            UnFrezeRoom();
        }
    }

    #region Event joining
    private void OnEnable()
    {
        foreach (DungenDoor door in m_doorsIn)
        {
            door.OnEnterRoom += RoomEntered;
            door.OnUnFreezeEntered += UnFrezeRoom;
        }
        foreach (DungenDoor door in m_doorsOut)
        {
            door.OnExitRoom += RoomExited;
            door.OnFrezzeExited += FrezzeExatingRoom;
        }
    }

    private void OnDisable()
    {
        foreach (DungenDoor door in m_doorsIn)
        {
            door.OnEnterRoom -= RoomEntered;
            door.OnUnFreezeEntered -= UnFrezeRoom;
        }
        foreach (DungenDoor door in m_doorsOut)
        {
            door.OnExitRoom -= RoomExited;
            door.OnFrezzeExited -= FrezzeExatingRoom;
        }
    }
    #endregion

    #region RoomJoiningControls
    private void RoomEntered()
    {
        //Called First when player enter room
        m_Camera.m_CurrentRoomType = m_RoomType;
        m_Camera.m_roomOragin = m_origin.transform.position;
        m_Camera.m_Locked = true;
        foreach (Trap trap in m_traps)
        {
            if (trap != null)
                trap.EnterRoomEnabled();
        }
    }

    private void UnFrezeRoom()
    {
        //Called after camra has finished moving and player unlocked
        m_Camera.m_Locked = false;
    }

    private void RoomExited()
    {
        //Called when the camra finishes moving

        foreach (Trap trap in m_traps)
        {
            if (trap != null)
            {
                trap.ExitRoomDisabled();
            }
        }
    }

    private void FrezzeExatingRoom()
    {
        //Called when room is first exated (Enamys etrar that need to be frozen in place befor being disabled after has moced)

    }
    #endregion

#if UNITY_EDITOR
    public GameObject tilePrefab;
    public GameObject lavaTilePrefab;
    public GameObject boxTilePrefab;
    public GameObject pitTilePrefab;

    public void ResetFloor()
    {
        Transform tiles = transform.Find("Floor").Find("Tiles");
        while (tiles.childCount > 0)
            DestroyImmediate(tiles.GetChild(0).gameObject);
        tiles.GetComponent<MeshCollider>().sharedMesh = null;

        Transform lava = transform.Find("Floor").Find("Lava");
        while (lava.childCount > 0)
            DestroyImmediate(lava.GetChild(0).gameObject);
        lava.GetComponent<MeshCollider>().sharedMesh = null;
    }

    public void GenerateRoom(DungeonEditorWindow.TileType[][] map)
    {
        ResetFloor();
        Transform tiles = transform.Find("Floor").Find("Tiles");
        Transform lava = transform.Find("Floor").Find("Lava");

        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == DungeonEditorWindow.TileType.Normal)
                {
                    GameObject tile = Instantiate(tilePrefab);
                    tile.transform.SetParent(tiles);
                    tile.transform.localPosition = new Vector3(y - 4.5f, tile.transform.localPosition.y, x - 9.5f);
                }
                else if (map[y][x] == DungeonEditorWindow.TileType.Lava)
                {
                    GameObject tile = Instantiate(lavaTilePrefab);
                    tile.transform.SetParent(lava);
                    tile.transform.localPosition = new Vector3(y - 4.5f, tile.transform.localPosition.y, x - 9.5f);
                }
                else if (map[y][x] == DungeonEditorWindow.TileType.Box)
                {
                    GameObject tile = Instantiate(boxTilePrefab);
                    tile.transform.SetParent(tiles);
                    tile.transform.localPosition = new Vector3(y - 4.5f, tile.transform.localPosition.y, x - 9.5f);
                }
                else if (map[y][x] == DungeonEditorWindow.TileType.Pit)
                {
                    GameObject tile = Instantiate(pitTilePrefab);
                    tile.transform.SetParent(tiles);
                    tile.transform.localPosition = new Vector3(y - 4.5f, tile.transform.localPosition.y, x - 9.5f);
                }
            }

        StaticBatchingUtility.Combine(tiles.gameObject);
        tiles.GetComponent<MeshCollider>().sharedMesh = tiles.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        StaticBatchingUtility.Combine(lava.gameObject);
        lava.GetComponent<MeshCollider>().sharedMesh = lava.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        /*MeshFilter[] meshFilters = new MeshFilter[floor.transform.childCount];
        int i = 0;
        foreach (Transform child in floor.transform)
        {
            meshFilters[i] = child.GetComponent<MeshFilter>();
            i++;
        }
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        floor.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        floor.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        floor.GetComponent<MeshCollider>().sharedMesh = floor.GetComponent<MeshFilter>().sharedMesh;*/
    }
#endif
}
