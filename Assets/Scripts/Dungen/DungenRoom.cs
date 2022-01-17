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
    public List<GameObject> m_Enamys;
    [SerializeField] List<Trap> m_traps;

    private void Start()
    {
        if(m_PlayerStartingRoom)
        {
            RoomEntered();
            m_Camera.transform.position = m_origin.transform.position;
            UnFrezeRoom();
        }
    }

    #region Event joining
    private void OnEnable()
    {
        foreach(DungenDoor door in m_doorsIn)
        {
            door.OnEnterRoom += RoomEntered;
            door.OnUnFreezeEntered += UnFrezeRoom;
        }
        foreach(DungenDoor door in m_doorsOut)
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
        foreach(Trap trap in m_traps)
        {
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
            trap.ExitRoomDisabled();
        }
    }

    private void FrezzeExatingRoom()
    {
        //Called when room is first exated (Enamys etrar that need to be frozen in place befor being disabled after has moced)

    }
#endregion
}
