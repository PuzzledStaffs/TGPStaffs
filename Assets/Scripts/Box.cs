using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public DungenRoom m_myRoom;

    Transform m_tiles;
    float xOffset;
    float yOffset;

    public float m_boxLerpTime;
    public Vector3 m_boxLerpStart;
    public Vector3 m_boxLerpEnd;
    public bool m_moving;

    // Start is called before the first frame update
    void Start()
    {
        m_tiles = m_myRoom.transform.Find("Floor").Find("Box Tiles");
        xOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -4.5f : -9.0f;
        yOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -9.5f : -19.0f;
        m_boxLerpStart = transform.localPosition;
        m_boxLerpEnd = transform.localPosition;
        m_boxLerpTime = 0.0f;
        m_moving = false;
        if (!GetTile(transform.localPosition))
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
                m_boxLerpEnd.y = transform.localPosition.y;
                transform.localPosition = Vector3.Lerp(m_boxLerpStart, m_boxLerpEnd, m_boxLerpTime);
                m_boxLerpTime += Time.deltaTime * 2.0f;
            } else
            {
                transform.localPosition = m_boxLerpEnd;
                m_moving = false;
            }

        }
    }

    public bool GetTile(Vector3 pos)
    {
        int pX = Mathf.RoundToInt(pos.x - xOffset), pZ = Mathf.RoundToInt(pos.z - yOffset);
        foreach (Transform tr in m_tiles)
        {
            int tX = Mathf.RoundToInt(tr.localPosition.x - xOffset), tZ = Mathf.RoundToInt(tr.localPosition.z - yOffset);
            if (pX == tX && pZ == tZ)
                return true;
        }
        return false;
    }

    public void Move(Vector2 mov)
    {
        m_boxLerpStart = transform.localPosition;
        m_boxLerpEnd = new Vector3(transform.localPosition.x + mov.x, transform.localPosition.y, transform.localPosition.z + mov.y);
        m_boxLerpTime = 0.0f;
        m_moving = true;
    }
}
