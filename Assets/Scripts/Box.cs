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

    private float m_boxLerpTime;
    private Vector3 m_boxLerpStart;
    [HideInInspector] public Vector3 m_boxLerpEnd;
    [HideInInspector] public bool m_moving;

    // Start is called before the first frame update
    void Start()
    {
        m_tiles = m_myRoom.transform.Find("Floor").Find("Box Tiles");
        xOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -4.5f : -9.0f;
        yOffset = m_myRoom.m_RoomType == RoomType.NORMAL ? -9.5f : -19.0f;
        m_boxLerpStart = transform.position;
        m_boxLerpEnd = transform.position;
        m_boxLerpTime = 0.0f;
        m_moving = false;
        if (!IsValidTile(new Vector3()))
        {
            Debug.LogError("Box " + name + " is not on a tile");
            //gameObject.SetActive(false);
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
            }
            else
            {
                transform.position = m_boxLerpEnd;
                m_moving = false;
            }
        }
    }

    public bool IsValidTile(Vector3 offset)
    {
        // Yes, the grid y is x and grid x is z. No, I don't remember why. I think it was because the room was created rotated, and so the top left tile actually is the top right in code
        int gridY = GetTileY(offset), gridX = GetTileX(offset);
        foreach (Transform tr in m_tiles)
        {
            int trGridY = Mathf.RoundToInt(tr.localPosition.x - xOffset), trGridX = Mathf.RoundToInt(tr.localPosition.z - yOffset);
            if (gridY == trGridY && gridX == trGridX)
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

    public int GetTileX()
    {
        return GetTileX(new Vector3());
    }

    public int GetTileY()
    {
        return GetTileY(new Vector3());
    }

    public int GetTileX(Vector3 offset)
    {
        Vector3 localOffset = transform.InverseTransformDirection(offset);
        return Mathf.RoundToInt(transform.localPosition.z - yOffset - localOffset.x);
    }

    public int GetTileY(Vector3 offset)
    {
        Vector3 localOffset = transform.InverseTransformDirection(offset);
        return Mathf.RoundToInt(transform.localPosition.x - xOffset - localOffset.z);
    }

    public bool Occupies(int tileX, int tileY)
    {
        return tileX == GetTileX() && tileY == GetTileY();
    }
}
