using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dispencer : Trap
{
    [SerializeField] private GameObject m_spawnedObject;
    [SerializeField][Min(1)] private int m_MaxiumInstances;
    List<GameObject> m_spawnedThings = new List<GameObject>();
    [SerializeField] TextMeshProUGUI m_TopText;
    [SerializeField] [Tooltip("The despencer will drop only the maximun instances, any more and it will not spawn")]
    private bool m_NoReset;

    private void Start()
    {
        m_TopText.text = m_MaxiumInstances.ToString();
    }

    public override void EnterRoomEnabled()
    {

    }
    public override void ExitRoomDisabled()
    {

    }

    public void SpawnItem()
    {
        //Spawn Item
        if(m_spawnedThings.Count < m_MaxiumInstances)
        {
            SpawnObject();
        }
        else if(!m_NoReset)
        {       
            Destroy(m_spawnedThings[0]);
            m_spawnedThings.RemoveAt(0);
            SpawnObject();
        }

        //Update Top Text
        m_TopText.text = (m_MaxiumInstances - m_spawnedThings.Count).ToString();
        if (m_TopText.text == "0")
        {
            m_TopText.color = Color.red;
            if(m_NoReset)
            {
                m_TopText.text = "X";
            }
        }
        else
        {
            m_TopText.color = Color.white;
        }
    }

    private void SpawnObject()
    {
        m_spawnedThings.Add(Instantiate(m_spawnedObject, transform.forward  + transform.position, transform.rotation));
    }
}
