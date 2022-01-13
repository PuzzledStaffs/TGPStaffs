using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungonCamaraControler : MonoBehaviour
{
    public bool m_Locked;
    public RoomType m_CurrentRoomType;
    public Vector3 m_roomOragin;
    [SerializeField] GameObject m_player;
    [SerializeField] private Vector2 m_BigRoomCamLimit;

    private void Start()
    {
        if(m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void LateUpdate()
    {
        if(!m_Locked)
        {
            switch(m_CurrentRoomType)
            {
                case RoomType.NORMAL:
                    break;
                case RoomType.BIG:
                    Vector3 camaraPos = m_player.transform.position;                 
                    
                    if(m_player.transform.position.x > m_roomOragin.x + m_BigRoomCamLimit.x)
                    {
                        camaraPos.x = m_roomOragin.x + m_BigRoomCamLimit.x;
                    }
                    else if(m_player.transform.position.x < m_roomOragin.x - m_BigRoomCamLimit.x)
                    {
                        camaraPos.x = m_roomOragin.x  - m_BigRoomCamLimit.x;
                    }

                    if (m_player.transform.position.z > m_roomOragin.z + m_BigRoomCamLimit.y)
                    {
                        camaraPos.z = m_roomOragin.z + m_BigRoomCamLimit.y;
                    }
                    else if (m_player.transform.position.z < m_roomOragin.z - m_BigRoomCamLimit.y)
                    {
                        camaraPos.z = m_roomOragin.z - m_BigRoomCamLimit.y;
                    }

                    camaraPos.y = m_roomOragin.y;
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