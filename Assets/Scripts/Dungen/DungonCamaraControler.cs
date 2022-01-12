using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungonCamaraControler : MonoBehaviour
{
    public bool m_Locked;
    public RoomType m_CurrentRoomType;
    
    void LateUpdate()
    {
        if(!m_Locked)
        {
            switch(m_CurrentRoomType)
            {
                case RoomType.NORMAL:
                    break;
                case RoomType.BIG:
                    break;
            }
        }
    }
}

[System.Serializable]
public enum RoomType
{
    NORMAL,
    BIG
}