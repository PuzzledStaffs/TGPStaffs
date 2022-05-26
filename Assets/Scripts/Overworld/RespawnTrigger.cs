using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.transform.position = new Vector3(
            PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionX,
            PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionY,
            PersistentPrefs.GetInstance().m_currentSaveFile.m_overworldRespawnPositionZ);
    }
}
