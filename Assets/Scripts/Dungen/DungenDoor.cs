using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenDoor : MonoBehaviour
{
    [SerializeField] private GameObject m_toRoomExitPoint;
    [SerializeField] private GameObject m_toRoomCameraMove;
    private DungenManager m_dungenManager;

    private void Awake()
    {
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (m_toRoomExitPoint != null)
            {
                
                StartCoroutine(m_dungenManager.MoveCameraCoroutine(m_toRoomCameraMove.transform.position));
                other.transform.position = m_toRoomExitPoint.transform.position;
            }
        }
    }
}
 