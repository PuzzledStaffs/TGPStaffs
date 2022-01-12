using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenRoom : MonoBehaviour
{
    public List<DungenDoor> m_doorsIn;
    public List<DungenDoor> m_doorsOut;
    public List<GameObject> m_Enamys;

    private void OnEnable()
    {
        foreach(DungenDoor door in m_doorsIn)
        {
            door.OnEnterRoom += OnRoomEntered;
        }
        foreach(DungenDoor door in m_doorsOut)
        {
            door.OnExitRoom += OnRoomExited;
        }
    }

    private void OnDisable()
    {
        foreach (DungenDoor door in m_doorsIn)
        {
            door.OnEnterRoom -= OnRoomEntered;
        }
        foreach (DungenDoor door in m_doorsOut)
        {
            door.OnExitRoom -= OnRoomExited;
        }
    }

    private void OnRoomEntered()
    {
        Debug.Log("Room: " + gameObject.name + " Entered");
        
    }

    private void OnRoomExited()
    {
        Debug.Log("Room: " + gameObject.name + " Exeted");

    }
}
