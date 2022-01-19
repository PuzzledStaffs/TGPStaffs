using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("RuntimeVariables")]
    [ReadOnly]
    public List<Collider> m_onPlate = new List<Collider>();
    public UnityEvent m_platePressed, m_plateUnpressed;

    // If a rigidbody with a collider enters trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Box")
        {
            m_onPlate.Add(other);
        }
        else if (other.tag == "KickBox")
        {
            m_onPlate.Add(other);
            other.GetComponentInParent<KickCube>().m_OnRemoveSelf += RemoveList;
        }
        if (m_onPlate.Count == 1)
            m_platePressed.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "KickBox")
            other.GetComponentInParent<KickCube>().m_OnRemoveSelf -= RemoveList;        
        m_onPlate.Remove(other);
        if (m_onPlate.Count == 0)
            m_plateUnpressed.Invoke();
    }

    private void RemoveList(Collider collider)
    {
        m_onPlate.Remove(collider);
        if (m_onPlate.Count == 0)
            m_plateUnpressed.Invoke();
    }
}
