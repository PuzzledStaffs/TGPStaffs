using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenDoor : MonoBehaviour ,IInteractable
{
    [SerializeField][Tooltip("This is the ExitPoint item on the door prefab, this is where the player go")] private GameObject m_toRoomExitPoint;
    [SerializeField][Tooltip("This is where the camara gose to")] private GameObject m_toRoomCameraMove;
    private DungenManager m_dungenManager;
    [Header("Door Closed Controls")]
    private Renderer m_doorRenderer;
    private BoxCollider m_DoorColider;
    [SerializeField] Material m_DoorClosed;
    [SerializeField] Material m_DoorOpen;
    [SerializeField] List<GameObject> m_locks;
    [SerializeField] public bool m_locked;
    public bool m_doorActive { get; protected set; }
    [SerializeField] private bool m_ClosedOnStart = false;

    public event Action OnEnterRoom;
    public event Action OnExitRoom;
    public event Action OnUnFreezeEntered;
    public event Action OnFrezzeExited;


    private void Awake()
    {
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();
        m_doorRenderer = transform.GetComponent<Renderer>();
        m_DoorColider = transform.GetComponent<BoxCollider>();

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
        other.GetComponent<PlayerController>().enabled = false;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        OnEnterRoom?.Invoke();
        OnFrezzeExited?.Invoke();
        other.transform.position = m_toRoomExitPoint.transform.position;
        other.transform.GetComponent<PlayerController>().m_respawnPosition = m_toRoomExitPoint.transform.position;
        yield return StartCoroutine(m_dungenManager.MoveCameraCoroutine(m_toRoomCameraMove.transform.position));
        OnUnFreezeEntered?.Invoke();
        OnExitRoom?.Invoke();
        other.GetComponent<PlayerController>().enabled = true;
    }

    public void Interact()
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

    public void OpenDoor()
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
    private void ShowLocks()
    {
        foreach (GameObject lockModel in m_locks)
        {
            lockModel.SetActive(true);
        }
    }

    private void HideLocks()
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