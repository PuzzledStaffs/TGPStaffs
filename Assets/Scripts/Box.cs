using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public DungenRoom m_myRoom;

    Transform m_tiles;
    float xOffset;
    float zOffset;

    public float m_boxLerpTime;
    public Vector3 m_boxLerpStart;
    public Vector3 m_boxLerpEnd;
    public bool m_moving;

    // Start is called before the first frame update
    void Start()
    {
        m_tiles = m_myRoom.transform.Find("Floor").Find("Box Tiles");
        xOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -4.5f : -9.0f;
        zOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -9.5f : -19.0f;
        m_boxLerpStart = transform.position;
        m_boxLerpEnd = transform.position;
        m_boxLerpTime = 0.0f;
        m_moving = false;
        if (!GetTile(new Vector3()))
        {
            Debug.LogError("Box is not on a tile");
            gameObject.SetActive(false);
            return;
        }
    }

    void Update()
    {
        if (m_moving)
        {
            if (m_boxLerpTime < 1.0f)
            {
                m_boxLerpEnd.y = transform.position.y;
                transform.position = Vector3.Lerp(m_boxLerpStart, m_boxLerpEnd, m_boxLerpTime);
                m_boxLerpTime += Time.deltaTime * 2.0f;
            } else
            {
                transform.position = m_boxLerpEnd;
                m_moving = false;
            }
        }
    }

    public bool GetTile(Vector3 offset)
    {
        Vector3 localOffset = transform.InverseTransformDirection(offset);
        // Keep in mind pX is grid y and pZ is grid x
        int pX = Mathf.RoundToInt(transform.localPosition.x - localOffset.x - xOffset), pZ = Mathf.RoundToInt(transform.localPosition.z + localOffset.z - zOffset);
        foreach (Transform tr in m_tiles)
        {
            int tX = Mathf.RoundToInt(tr.localPosition.x - xOffset), tZ = Mathf.RoundToInt(tr.localPosition.z - zOffset);
            if (pX == tX && pZ == tZ)
                return true;
        }
        return false;
    }

    public void Move(Vector3 mov)
    {
        m_boxLerpStart = transform.position;
        // x and y are swapped beause localPosition weirdness, mov.x needs to be inverted
        m_boxLerpEnd = new Vector3(transform.position.x + mov.z, transform.position.y, transform.position.z + mov.x);
        m_boxLerpTime = 0.0f;
        m_moving = true;
    }
}
