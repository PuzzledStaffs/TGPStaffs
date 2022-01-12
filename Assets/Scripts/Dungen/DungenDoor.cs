using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenDoor : MonoBehaviour
{
    [SerializeField] private GameObject m_toRoomExitPoint;
    [SerializeField] private GameObject m_toRoomCameraMove;
    private DungenManager m_dungenManager;
    [Header("Door Closed Controls")]
    private Renderer m_doorRenderer;
    private BoxCollider m_DoorColider;
    [SerializeField] Material m_DoorClosed;
    [SerializeField] Material m_DoorOpen;
    public bool m_doorActive { get; protected set; }

    public event Action OnEnterRoom;
    public event Action OnExitRoom;


    private void Awake()
    {
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();
        m_doorRenderer = transform.GetComponent<Renderer>();
        m_DoorColider = transform.GetComponent<BoxCollider>();
        OpenDoor();
        if (m_toRoomCameraMove == null && m_toRoomExitPoint == null)
        {
            CloseDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (m_toRoomExitPoint != null)
            {
                OnEnterRoom?.Invoke();
                StartCoroutine(m_dungenManager.MoveCameraCoroutine(m_toRoomCameraMove.transform.position));
                other.transform.position = m_toRoomExitPoint.transform.position;
                OnExitRoom?.Invoke();
            }
        }
    }

    #region Door Controls

    public void OpenDoor()
    {
        if (m_toRoomCameraMove != null && m_toRoomExitPoint != null)
        {
            m_doorActive = true;
            m_doorRenderer.material = m_DoorOpen;
            m_DoorColider.isTrigger = true;
        }
        else
        {
            Debug.LogWarning("Door: " + transform.parent.name + " " + gameObject.name + "Is missing a destination");
        }
    }

    public void CloseDoor()
    {
        m_doorActive = false;
        m_doorRenderer.material = m_DoorClosed;
        m_DoorColider.isTrigger = false;
    }

    #endregion
}
