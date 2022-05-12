using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungonCamaraControler : MonoBehaviour
{
    [FormerlySerializedAs("m_Locked")]
    public bool m_locked;
    [FormerlySerializedAs("m_CurrentRoomType")]
    public RoomType m_currentRoomType;
    [FormerlySerializedAs("m_roomOragin")]
    public Vector3 m_roomOrigin;
    [SerializeField] GameObject m_player;
    [FormerlySerializedAs("m_BigRoomCamLimit")]
    [SerializeField] private Vector2 m_bigRoomCamLimit;

    private void Start()
    {
        if(m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void LateUpdate()
    {
        if(!m_locked)
        {
            switch(m_currentRoomType)
            {
                case RoomType.NORMAL:
                    break;
                case RoomType.BIG:
                    Vector3 camaraPos = m_player.transform.position;                 
                    
                    if(m_player.transform.position.x > m_roomOrigin.x + m_bigRoomCamLimit.x)
                    {
                        camaraPos.x = m_roomOrigin.x + m_bigRoomCamLimit.x;
                    }
                    else if(m_player.transform.position.x < m_roomOrigin.x - m_bigRoomCamLimit.x)
                    {
                        camaraPos.x = m_roomOrigin.x  - m_bigRoomCamLimit.x;
                    }

                    if (m_player.transform.position.z > m_roomOrigin.z + m_bigRoomCamLimit.y)
                    {
                        camaraPos.z = m_roomOrigin.z + m_bigRoomCamLimit.y;
                    }
                    else if (m_player.transform.position.z < m_roomOrigin.z - m_bigRoomCamLimit.y)
                    {
                        camaraPos.z = m_roomOrigin.z - m_bigRoomCamLimit.y;
                    }

                    camaraPos.y = m_roomOrigin.y;
                    transform.position = camaraPos;
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