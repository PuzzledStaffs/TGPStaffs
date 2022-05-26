using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector3 pos = transform.Find("Respawn Point").position;
        PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionX = pos.x;
        PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionY = pos.y;
        PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionZ = pos.z;
    }
}
