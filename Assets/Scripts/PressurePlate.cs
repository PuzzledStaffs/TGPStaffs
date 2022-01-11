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
            m_onPlate.Add(other);
        if (m_onPlate.Count > 0)
            m_platePressed.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (m_onPlate.Contains(other))
            m_onPlate.Remove(other);
        if (m_onPlate.Count == 0)
            m_plateUnpressed.Invoke();
    }
}
