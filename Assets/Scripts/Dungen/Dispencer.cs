using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispencer : Trap
{
    [SerializeField] private GameObject m_spawnedObject;
    [SerializeField][Min(1)] private int m_MaxiumInstances;
    [SerializeField] [ReadOnly] List<GameObject> m_spawnedThings = new List<GameObject>();


    public override void EnterRoomEnabled()
    {

    }
    public override void ExitRoomDisabled()
    {

    }

    public void SpawnItem()
    {
        if(m_spawnedThings.Count < m_MaxiumInstances)
        {
            m_spawnedThings.Add(Instantiate(m_spawnedObject, transform.forward/2 + transform.position, transform.rotation));
        }
        else
        {
            Destroy(m_spawnedThings[0]);
            m_spawnedThings.RemoveAt(0);
            m_spawnedThings.Add(Instantiate(m_spawnedObject, transform.forward / 2 + transform.position, transform.rotation));
        }
    }
}
