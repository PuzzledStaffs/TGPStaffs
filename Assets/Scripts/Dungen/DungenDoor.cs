using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenDoor : MonoBehaviour ,IInteractable
{
    [Header("Door Oriatation and locating")]
    [SerializeField] public DoorLoaction m_doorLoaction;
    [Tooltip("This is the ExitPoint item on the door prefab, this is where the player go")] protected GameObject m_toRoomExitPoint;
    [Tooltip("This is where the camara gose to")] protected GameObject m_toRoomCameraMove;
    [SerializeField] public GameObject m_ownExitPoint;
    [SerializeField] public GameObject m_ownCamraNode;

    protected DungenManager m_dungenManager;

    [Header("Door Closed Controls")]
    protected Renderer m_doorRenderer;
    protected BoxCollider m_DoorColider;

    [Header("Door Cosmetics")]
    [SerializeField] Material m_DoorClosed;
    [SerializeField] protected Material m_DoorOpen;
    [SerializeField] List<GameObject> m_locks;

    [Header("Door Controles")]
    [SerializeField] public bool m_locked;
    public bool m_doorActive { get; protected set; }
    [SerializeField] protected bool m_ClosedOnStart = false;

    public event Action OnEnterRoom;
    public event Action OnExitRoom;
    public event Action OnUnFreezeEntered;
    public event Action OnFrezzeExited;


    private void Awake()
    {
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();
        m_doorRenderer = transform.GetComponent<Renderer>();
        m_DoorColider = transform.GetComponent<BoxCollider>();

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

        if (m_ClosedOnStart || m_locked)
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

    public virtual void Interact()
    {
        if (m_locked && m_toRoomCameraMove != null && m_toRoomExitPoint != null)
        {
            if (m_dungenManager.UseKey())
            {
                m_locked = false;
                HideLocks();
                OpenDoor();
            }
        }
    }

    #region Door Controls

    public virtual void OpenDoor()
    {
        if (m_toRoomCameraMove != null && m_toRoomExitPoint != null && !m_locked)
        {
            m_doorActive = true;
            m_doorRenderer.material = m_DoorOpen;
            m_DoorColider.isTrigger = true;
          
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
        m_doorActive = false;
        m_doorRenderer.material = m_DoorClosed;
        m_DoorColider.isTrigger = false;
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