using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Dispencer : MonoBehaviour
{
    [SerializeField] private GameObject m_spawnedObject;
    [SerializeField][Range(1,99)] [FormerlySerializedAs("m_MaxiumInstances")] private int m_maxiumInstances = 1;
    private int m_spawnedObjectsCount;
    List<GameObject> m_spawnedThings = new List<GameObject>();
    [SerializeField] [FormerlySerializedAs("m_TopText")] TextMeshProUGUI m_TopText;
    [SerializeField] [Tooltip("The despencer will drop only the maximum instances, any more and it will not spawn")]
    [FormerlySerializedAs("m_NoReset")] private bool  m_NoReset;

    private void Start()
    {
        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasIntFlag(gameObject.scene.name + "_Dispenser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name))
        {
            m_spawnedObjectsCount = PersistentPrefs.GetInstance().m_currentSaveFile.GetIntFlag(gameObject.scene.name + "_Dispenser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name);
        }
        m_TopText.text = m_maxiumInstances.ToString();
    }

   

    public void SpawnItem()
    {
        //Spawn Item
        if(m_spawnedObjectsCount < m_maxiumInstances)
        {
            SpawnObject();
            PersistentPrefs.GetInstance().m_currentSaveFile.SetIntFlag(gameObject.scene.name + "_Dispenser_" + gameObject.transform.parent.parent.name + "_" + gameObject.name, m_spawnedObjectsCount);
        }
        else if(!m_NoReset)
        {       
            Destroy(m_spawnedThings[0]);
            m_spawnedThings.RemoveAt(0);
            SpawnObject();
        }

        //Update Top Text
        m_TopText.text = (m_maxiumInstances - m_spawnedThings.Count).ToString();
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
        m_spawnedObjectsCount++;
    }
}
