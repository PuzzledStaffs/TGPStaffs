using UnityEngine.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungenDoor : MonoBehaviour ,IAltInteractable
{
    [Header("Door Oriatation and locating")]
    [SerializeField][FormerlySerializedAs("m_doorLoaction")] public DoorLoaction m_doorLocation;
    [Tooltip("This is the ExitPoint item on the door prefab, this is where the player go")] protected GameObject m_toRoomExitPoint;
    [Tooltip("This is where the camara gose to")] protected GameObject m_toRoomCameraMove;
    [SerializeField] public GameObject m_ownExitPoint;
    [SerializeField] public GameObject m_ownCamraNode;

    protected DungenManager m_dungenManager;

    [Header("Door Closed Controls")]
   
   [FormerlySerializedAs("m_DoorColider")] protected BoxCollider m_doorCollider;

    [Header("Door Cosmetics")]
    
    [SerializeField] List<GameObject> m_locks;
    [SerializeField] protected List<GameObject> m_bars;


    [Header("Door Controles")]
    [SerializeField] protected bool m_locked;
    public bool m_doorActive { get; protected set; }
    [SerializeField][FormerlySerializedAs("m_ClosedOnStart")] protected bool m_closedOnStart = false;
    public string m_textForNextScene;

    public event Action OnEnterRoom;
    public event Action OnExitRoom;
    public event Action OnUnFreezeEntered;
    public event Action OnFrezzeExited;


    private void Awake()
    {
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();       
        m_doorCollider = transform.GetComponent<BoxCollider>();

        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_DoorOpen_" + gameObject.transform.parent.name + "_" + gameObject.name))
        {
            m_closedOnStart = false;
        }
        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_DoorUnlocked_" + gameObject.transform.parent.name + "_" + gameObject.name))
        {
            m_locked = false;
        }

        CheckDoorSet();
    }

    public void UpdateExit(GameObject ExitPoint, GameObject CameraNode)
    {
        m_toRoomExitPoint = ExitPoint;
        m_toRoomCameraMove = CameraNode;
        CheckDoorSet();
    }

    protected virtual void CheckDoorSet()
    {
        if (m_locked && m_toRoomCameraMove != null && m_toRoomExitPoint != null)
        {
            ShowLocks();
        }
        else
        {
            HideLocks();
        }

        if (m_closedOnStart || m_locked)
        {
            CloseDoor();

        }
        else if (m_toRoomCameraMove == null || m_toRoomExitPoint == null)
        {
            CloseDoor();

        }
        else
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
                           
            StartCoroutine(MoveRoomCoroutine(other));
            
        }
    }

    protected virtual IEnumerator MoveRoomCoroutine(Collider other)
    {
        other.GetComponent<PlayerController>().FreezeMovement();
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        OnEnterRoom?.Invoke();
        OnFrezzeExited?.Invoke();
        other.transform.position = m_toRoomExitPoint.transform.position;
        other.transform.GetComponent<PlayerController>().m_respawnPosition = m_toRoomExitPoint.transform.position;
        yield return StartCoroutine(m_dungenManager.MoveCameraCoroutine(m_toRoomCameraMove.transform.position));
        OnUnFreezeEntered?.Invoke();
        OnExitRoom?.Invoke();
        other.GetComponent<PlayerController>().UnFreezeMovement();
    }

    public virtual void AltInteract()
    {
        if (m_locked && m_toRoomCameraMove != null && m_toRoomExitPoint != null)
        {
            if (m_dungenManager.UseKey())
            {
                m_locked = false;
                PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_DoorUnlocked_" + gameObject.transform.parent.name + "_" + gameObject.name);
                HideLocks();
                OpenDoor();
            }
        }
    }
    
    public InteractInfo CanInteract()
    {
        return new InteractInfo(m_locked, "Unlock Door", 3);
    }
    

    #region Door Controls

    public virtual void OpenDoor()
    {
        PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_DoorOpen_" + gameObject.transform.parent.name + "_" + gameObject.name);

        if (m_toRoomCameraMove != null && m_toRoomExitPoint != null && !m_locked)
        {
            m_doorActive = true;
            foreach(GameObject bar in m_bars)
            {
                bar.active = false;
            }
            m_doorCollider.isTrigger = true;
          
        }
        else if (m_locked)
        {
            return;
        }
        else
        {
            Debug.LogWarning("Door: " + transform.parent.name + " " + gameObject.name + "Is missing a destination");
            return;
        }
    }

    public void CloseDoor()
    {
        PersistentPrefs.GetInstance().m_currentSaveFile.RemoveFlag(gameObject.scene.name + "_DoorOpen_" + gameObject.transform.parent.name + "_" + gameObject.name);

        m_doorActive = false;
        foreach (GameObject bar in m_bars)
        {
            bar.active = true;
        }
        m_doorCollider.isTrigger = false;
    }

    #endregion

    #region LockedControls
    protected void ShowLocks()
    {
        foreach (GameObject lockModel in m_locks)
        {
            lockModel.SetActive(true);
        }
    }

    protected void HideLocks()
    {
        foreach (GameObject lockModel in m_locks)
        {
            lockModel.SetActive(false);
        }
    }

    #endregion
}

public enum DoorStates
{
    OPENED,
    LOCKED,
    UNIVALIBLE
}

public enum DoorLoaction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}