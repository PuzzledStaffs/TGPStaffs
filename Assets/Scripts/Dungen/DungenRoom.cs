using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DungenRoom : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("m_Camera")] private DungeonCameraController m_camera;
    [SerializeField][FormerlySerializedAs("m_RoomType")] public RoomType m_roomType;
    [SerializeField] public GameObject m_origin;
    [SerializeField][FormerlySerializedAs("m_PlayerStartingRoom")] public bool m_playerStartingRoom = false;
    public List<DungenDoor> m_doorsIn;
    public List<DungenDoor> m_doorsOut;

    [FormerlySerializedAs("m_Enemies")]List<EnemyController> m_enemies;   
    [SerializeField][FormerlySerializedAs("m_EneamyPerent")] private GameObject m_enemyParent;
    Trap[] m_traps;
    [SerializeField][FormerlySerializedAs("m_TrapPernet")] private GameObject m_trapParent;

    [FormerlySerializedAs("m_roomCleard")]  public UnityEvent m_roomCleared;
    int m_enemyCount;
    PauseMenu m_pauseMenu;
    public bool m_playerInRoom;

    private DungenManager m_dungeonManager;

    private PlayerController m_playerController;

    private void Awake()
    {
        m_traps = m_trapParent.GetComponentsInChildren<Trap>();
        m_enemies = new List<EnemyController>(m_enemyParent.GetComponentsInChildren<EnemyController>());
        //m_enemiesIdle = new List<IdleState>(m_EneamyPerent.GetComponentsInChildren<IdleState>());
        m_enemyCount = m_enemies.Count;
        m_dungeonManager = GameObject.FindGameObjectWithTag("dungeonManager").GetComponent<DungenManager>();
        m_camera = m_dungeonManager.m_dungeonCam.GetComponent<DungeonCameraController>();

        SceneManager.sceneLoaded += onSceneLoad;

        m_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //m_playerController.m_Death1 += playerDeath;
    }

    private void Start()
    {

        if (m_enemies != null)
        foreach (EnemyController enemy in m_enemies)
        {
            if (enemy.IsDead())
                continue;
            enemy.ChangeState(State.StateType.IDLE);
            enemy.m_deadEvent += EnameyDie;
            //enemy.m_manager.m_idle.enabled = true;
            //enemy.m_fieldOfView.enabled = false;
        }
        //Set door
        foreach (DungenDoor door in m_doorsIn)
        {
            
            switch(door.m_doorLocation)
            {
                case DoorLoaction.NORTH:
                    m_doorsOut[1].UpdateExit(door.m_ownExitPoint, door.m_ownCamraNode);
                    break;
                case DoorLoaction.EAST:
                    m_doorsOut[2].UpdateExit(door.m_ownExitPoint, door.m_ownCamraNode);
                    break;
                case DoorLoaction.SOUTH:
                    m_doorsOut[0].UpdateExit(door.m_ownExitPoint, door.m_ownCamraNode);
                    break;
                case DoorLoaction.WEST:
                    m_doorsOut[3].UpdateExit(door.m_ownExitPoint, door.m_ownCamraNode);
                    break;
            }
            
        }

        m_pauseMenu = GameObject.FindObjectOfType<PauseMenu>();
        m_pauseMenu.m_pause += FreezeExitingRoom;
        m_pauseMenu.m_unPause += UnFrezeRoom;

        if (m_enemyCount <= 0)
        {
            m_roomCleared?.Invoke();
        }

        if (m_playerStartingRoom)
        {
            if (m_dungeonManager != null)
            {
                m_dungeonManager.SetStartingRoom(this);
            }
        }
        else
        {
            FreezeExitingRoom();
            RoomExited();
        }
    }

    private void playerDeath()
    {
        if(m_playerInRoom)
        {
            FreezeExitingRoom();
        }
    }

    void onSceneLoad(Scene scene, LoadSceneMode mode)
    {
        /*
        if (m_PlayerStartingRoom)
        {
            DungenManager manager = GameObject.FindWithTag("dungeonManager").GetComponent<DungenManager>();
            if (manager != null)
                manager.m_startingRoom = this;
        } */
    }

    public void StartRoom()
    {
        UnFrezeRoom();
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
            door.OnFrezzeExited += FreezeExitingRoom;
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
            door.OnFrezzeExited -= FreezeExitingRoom;
        }
    }
    
    #endregion

    #region RoomJoiningControls
    public void RoomEntered()
    {
        PersistentPrefs.GetInstance().m_currentSaveFile.m_currentDungeonRoom = gameObject.name;

        //Called First when player enter room
        m_camera.m_currentRoomType = m_roomType;
        m_camera.m_roomOrigin = m_origin.transform.position;
        m_camera.m_locked = true;
        
        m_playerInRoom = true;
    }

    public void UnFrezeRoom()
    {
        if (m_playerInRoom)
        {
            //Called after camra has finished moving and player unlocked
            m_camera.m_locked = false;
            foreach (EnemyController enemy in m_enemies)
            {
                if (enemy == null || enemy.IsDead())
                    continue;
                enemy.ChangeState(State.StateType.CHASE);
                enemy.toggleHeathBar(true);
                //enemy.m_manager.m_idle.m_isIdle = false;
                //enemy.m_manager.m_idle.enabled = false;

                //enemy.m_fieldOfView.enabled = true;
            }
            foreach (Trap trap in m_traps)
            {
                if (trap != null)
                    trap.EnterRoomEnabled();
            }
        }
    }

    private void RoomExited()
    {
        //Called when the camra finishes moving
        m_playerInRoom = false;
        
    }

    /// <summary>
    /// Called To Frezze room
    /// </summary>
    public void FreezeExitingRoom()
    {

        //Called when room is first exated (Enamys etrar that need to be frozen in place befor being disabled after has moced)
        foreach (EnemyController enemy in m_enemies)
        {
            if (enemy == null || enemy.IsDead())
                continue;
            enemy.ChangeState(State.StateType.IDLE);
            enemy.toggleHeathBar(false);
            //enemy.m_manager.m_attack.enabled = false;
            //enemy.m_manager.m_idle.m_isIdle = true;
            //enemy.m_manager.m_idle.enabled = true;

            //enemy.m_fieldOfView.enabled = false;
        }


        foreach (Trap trap in m_traps)
        {
            if (trap != null)
            {
                trap.ExitRoomDisabled();
            }
        }
      
    }
    #endregion

    private void EnameyDie(GameObject enemy)
    {
        EnemyController controller = enemy.GetComponent<EnemyController>();
        m_enemyCount--;
        m_enemies.Remove(controller);
        controller.m_deadEvent -= EnameyDie;
        //m_enemiesIdle.Remove(enemy.GetComponent<IdleState>());
        if (m_enemyCount <= 0)
        {
            m_roomCleared?.Invoke();
        }
    }

    public void AddEnemy(EnemyController enemy)
    {
        m_enemyCount++;
        m_enemies.Add(enemy);
        enemy.ChangeState(State.StateType.IDLE);
        enemy.m_deadEvent += EnameyDie;
    }

    #region Floor Generation
#if UNITY_EDITOR
    public GameObject tilePrefab;
    public GameObject lavaTilePrefab;
    public GameObject boxTilePrefab;
    public GameObject pitTilePrefab;
    public PhysicMaterial frictionless;

    public void ResetFloor()
    {
        Transform tiles = transform.Find("Floor").Find("Tiles");
        while (tiles.childCount > 0)
            DestroyImmediate(tiles.GetChild(0).gameObject);
        tiles.GetComponent<MeshFilter>().sharedMesh = null;
        tiles.GetComponent<MeshCollider>().sharedMesh = null;

        Transform lava = transform.Find("Floor").Find("Lava");
        while (lava.childCount > 0)
            DestroyImmediate(lava.GetChild(0).gameObject);
        while (lava.GetComponent<BoxCollider>() != null)
            DestroyImmediate(lava.GetComponent<BoxCollider>());
        lava.GetComponent<MeshFilter>().sharedMesh = null;
        lava.GetComponent<MeshCollider>().sharedMesh = null;

        Transform pit = transform.Find("Floor").Find("Pit");
        while (pit.childCount > 0)
            DestroyImmediate(pit.GetChild(0).gameObject);
        while (pit.GetComponent<BoxCollider>() != null)
            DestroyImmediate(pit.GetComponent<BoxCollider>());
        pit.GetComponent<MeshFilter>().sharedMesh = null;
        pit.GetComponent<MeshCollider>().sharedMesh = null;

        Transform box = transform.Find("Floor").Find("Box Tiles");
        while (box.childCount > 0)
            DestroyImmediate(box.GetChild(0).gameObject);
        while (box.GetComponent<BoxCollider>() != null)
            DestroyImmediate(box.GetComponent<BoxCollider>());
        box.GetComponent<MeshFilter>().sharedMesh = null;
        box.GetComponent<MeshCollider>().sharedMesh = null;
    }

    /// <summary>
    /// Creates the Floor Tiles for this Dungeon Room using the given TileType array
    /// </summary>
    public void GenerateRoom(DungeonEditorWindow.TileType[][] map)
    {
        ResetFloor();
        Transform tiles = transform.Find("Floor").Find("Tiles");
        Transform lava = transform.Find("Floor").Find("Lava");
        Transform pit = transform.Find("Floor").Find("Pit");
        Transform boxTiles = transform.Find("Floor").Find("Box Tiles");

        float xOffset = m_roomType == RoomType.NORMAL ? -4.5f : -9.0f;
        float yOffset = m_roomType == RoomType.NORMAL ? -9.5f : -19.0f;

        // Loop through all tiles and create them
        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[y].Length; x++)
            {
                switch (map[y][x])
                {
                    case DungeonEditorWindow.TileType.Normal:
                        {
                            GameObject tile = Instantiate(tilePrefab);
                            tile.transform.SetParent(tiles);
                            tile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            tile.transform.localPosition = new Vector3(y + xOffset, tile.transform.localPosition.y, x + yOffset);
                            break;
                        }
                    case DungeonEditorWindow.TileType.Lava:
                        {
                            GameObject tile = Instantiate(lavaTilePrefab);
                            tile.transform.SetParent(lava);
                            tile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            tile.transform.localPosition = new Vector3(y + xOffset, tile.transform.localPosition.y, x + yOffset);
                            break;
                        }
                    case DungeonEditorWindow.TileType.Box:
                        {
                            GameObject tile = Instantiate(boxTilePrefab);
                            tile.transform.SetParent(boxTiles);
                            tile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            tile.transform.localPosition = new Vector3(y + xOffset, tile.transform.localPosition.y, x + yOffset);
                            break;
                        }
                    case DungeonEditorWindow.TileType.Pit:
                        {
                            GameObject tile = Instantiate(pitTilePrefab);
                            tile.transform.SetParent(pit);
                            tile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            tile.transform.localPosition = new Vector3(y + xOffset, tile.transform.localPosition.y, x + yOffset);
                            OrganiseTile(map, x, y, tile.transform.Find("Model"));
                            tile.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                            break;
                        }
                }
            }

        if (tiles.childCount > 0)
            CombineMesh(tiles);

        if (lava.childCount > 0)
        {
            foreach (BoxCollider col in lava.GetComponentsInChildren<BoxCollider>())
            {
                BoxCollider box = lava.gameObject.AddComponent<BoxCollider>();
                box.center = -(col.center - col.gameObject.transform.localPosition);
                box.size = new Vector3(col.size.x * col.gameObject.transform.localScale.x,
                    col.size.y * col.gameObject.transform.localScale.y,
                    col.size.z * col.gameObject.transform.localScale.z);
                box.isTrigger = true;
            }
            CombineMesh(lava);
        }

        if (pit.childCount > 0)
        {
            foreach (BoxCollider col in pit.GetComponentsInChildren<BoxCollider>())
            {
                BoxCollider box = pit.gameObject.AddComponent<BoxCollider>();
                box.center = -(col.center - col.gameObject.transform.localPosition);
                box.center += new Vector3(0.0f, 2 * col.center.y, 0.0f);
                box.size = new Vector3(col.size.x * col.gameObject.transform.localScale.x,
                    col.size.y * col.gameObject.transform.localScale.y,
                    col.size.z * col.gameObject.transform.localScale.z);
                box.isTrigger = true;
            }
            CombineMesh(pit);
        }

        if (boxTiles.childCount > 0)
        {
            /*foreach (BoxCollider col in boxTiles.GetComponentsInChildren<BoxCollider>())
            {
                BoxCollider box = boxTiles.gameObject.AddComponent<BoxCollider>();
                box.center = -(col.center - col.gameObject.transform.localPosition);
                box.center += new Vector3(0.0f, 2 * col.center.y, 0.0f);
                box.size = new Vector3(col.size.x * col.gameObject.transform.localScale.x,
                    col.size.y * col.gameObject.transform.localScale.y,
                    col.size.z * col.gameObject.transform.localScale.z);
                box.material = frictionless;
            }
            CombineMesh(boxTiles);*/
        }
    }

    public void CombineMesh(Transform parent)
    {
        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        foreach (Transform tile in parent.transform)
            foreach (MeshFilter child in tile.GetComponentsInChildren<MeshFilter>())
                if (child.gameObject.activeSelf)
                    meshFilterList.Add(child);

        Matrix4x4 parentTransform = parent.transform.worldToLocalMatrix;
        MeshFilter[] meshFilters = meshFilterList.ToArray();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = parentTransform * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        parent.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        parent.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        parent.GetComponent<MeshFilter>().sharedMesh.Optimize();
        if (parent.GetComponent<MeshCollider>() != null)
            parent.GetComponent<MeshCollider>().sharedMesh = parent.GetComponent<MeshFilter>().sharedMesh;

        while (parent.childCount > 0)
            DestroyImmediate(parent.GetChild(0).gameObject);
    }

    public void OrganiseTile(DungeonEditorWindow.TileType[][] map, int tx, int ty, Transform model)
    {
        bool[][] matrix = new bool[3][];
        for (int y = 0; y < 3; y++)
        {
            matrix[y] = new bool[3];
            for (int x = 0; x < 3; x++)
            {
                if (ty + y - 1 < 0 || ty + y - 1 >= map.Length ||
                    tx + x - 1 < 0 || tx + x - 1 >= map[ty + y - 1].Length ||
                    map[ty + y - 1][tx + x - 1] != map[ty][tx])
                {
                    matrix[y][x] = false;
                }
                else
                    matrix[y][x] = true;
            }
        }

        if (!matrix[0][1])
            model.Find("up").gameObject.SetActive(true);
        if (!matrix[2][1])
            model.Find("down").gameObject.SetActive(true);
        if (!matrix[1][0])
            model.Find("left").gameObject.SetActive(true);
        if (!matrix[1][2])
            model.Find("right").gameObject.SetActive(true);

        // If Up, and Left, and not Up Left Corner
        if (matrix[0][1] && matrix[1][0] && !matrix[0][0])
            model.Find("up left corner base").gameObject.SetActive(true);
        // If Up, and Right, and not Up Right Corner
        if (matrix[0][1] && matrix[1][2] && !matrix[0][2])
            model.Find("up right corner base").gameObject.SetActive(true);
        // If Down, and Left, and not Down Left Corner
        if (matrix[2][1] && matrix[1][0] && !matrix[2][0])
            model.Find("down left corner base").gameObject.SetActive(true);
        // If Down, and Right, and not Down Right Corner
        if (matrix[2][1] && matrix[1][2] && !matrix[2][2])
            model.Find("down right corner base").gameObject.SetActive(true);

        // If not Up, and not Left, and not Up Left Corner
        if (!matrix[0][1] && !matrix[1][0] && !matrix[0][0])
        {
            model.Find("up left").gameObject.SetActive(true);
            model.Find("up left base").gameObject.SetActive(true);
            model.Find("up left corner base").gameObject.SetActive(true);
            model.Find("up left corner").gameObject.SetActive(true);
            model.Find("left up base").gameObject.SetActive(true);
            model.Find("left up").gameObject.SetActive(true);
        }
        // If not Up, and not Right, and not Up Right Corner
        if (!matrix[0][1] && !matrix[1][2] && !matrix[0][2])
        {
            model.Find("up right").gameObject.SetActive(true);
            model.Find("up right base").gameObject.SetActive(true);
            model.Find("up right corner base").gameObject.SetActive(true);
            model.Find("up right corner").gameObject.SetActive(true);
            model.Find("right up base").gameObject.SetActive(true);
            model.Find("right up").gameObject.SetActive(true);
        }
        // If not Down, and not Left, and not Down Left Corner
        if (!matrix[2][1] && !matrix[1][0] && !matrix[2][0])
        {
            model.Find("down left").gameObject.SetActive(true);
            model.Find("down left base").gameObject.SetActive(true);
            model.Find("down left corner base").gameObject.SetActive(true);
            model.Find("down left corner").gameObject.SetActive(true);
            model.Find("left down base").gameObject.SetActive(true);
            model.Find("left down").gameObject.SetActive(true);
        }
        // If not Down, and not Right, and not Down Right Corner
        if (!matrix[2][1] && !matrix[1][2] && !matrix[2][2])
        {
            model.Find("down right").gameObject.SetActive(true);
            model.Find("down right base").gameObject.SetActive(true);
            model.Find("down right corner base").gameObject.SetActive(true);
            model.Find("down right corner").gameObject.SetActive(true);
            model.Find("right down base").gameObject.SetActive(true);
            model.Find("right down").gameObject.SetActive(true);
        }

        // If Up, and not Left, and not Up Left Corner
        if (matrix[0][1] && !matrix[1][0] && !matrix[0][0])
        {
            model.Find("left up").gameObject.SetActive(true);
            model.Find("left up base").gameObject.SetActive(true);
        }
        // If Up, and not Right, and not Up Right Corner
        if (matrix[0][1] && !matrix[1][2] && !matrix[0][2])
        {
            model.Find("right up").gameObject.SetActive(true);
            model.Find("right up base").gameObject.SetActive(true);
        }
        // If Down, and not Left, and not Down Left Corner
        if (matrix[2][1] && !matrix[1][0] && !matrix[2][0])
        {
            model.Find("left down").gameObject.SetActive(true);
            model.Find("left down base").gameObject.SetActive(true);
        }
        // If Down, and not Right, and not Down Right Corner
        if (matrix[2][1] && !matrix[1][2] && !matrix[2][2])
        {
            model.Find("right down").gameObject.SetActive(true);
            model.Find("right down base").gameObject.SetActive(true);
        }
        // If not Up, and Left, and not Up Left Corner
        if (!matrix[0][1] && matrix[1][0] && !matrix[0][0])
        {
            model.Find("up left").gameObject.SetActive(true);
            model.Find("up left base").gameObject.SetActive(true);
        }
        // If not Up, and Right, and not Up Right Corner
        if (!matrix[0][1] && matrix[1][2] && !matrix[0][2])
        {
            model.Find("up right").gameObject.SetActive(true);
            model.Find("up right base").gameObject.SetActive(true);
        }
        // If not Down, and Left, and not Down Left Corner
        if (!matrix[2][1] && matrix[1][0] && !matrix[2][0])
        {
            model.Find("down left").gameObject.SetActive(true);
            model.Find("down left base").gameObject.SetActive(true);
        }
        // If not Down, and Right, and not Down Right Corner
        if (!matrix[2][1] && matrix[1][2] && !matrix[2][2])
        {
            model.Find("down right").gameObject.SetActive(true);
            model.Find("down right base").gameObject.SetActive(true);
        }

        // If not Up, and not Left, but Up Left Corner
        if (!matrix[0][1] && !matrix[1][0] && matrix[0][0])
        {
            model.Find("up left base").gameObject.SetActive(true);
            model.Find("left up base").gameObject.SetActive(true);
        }
        // If not Up, and not Right, but Up Right Corner
        if (!matrix[0][1] && !matrix[1][2] && matrix[0][2])
        {
            model.Find("up right base").gameObject.SetActive(true);
            model.Find("right up base").gameObject.SetActive(true);
        }
    }
#endif
    #endregion
}


