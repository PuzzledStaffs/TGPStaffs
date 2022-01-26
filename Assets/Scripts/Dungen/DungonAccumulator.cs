using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class DungonAccumulator : MonoBehaviour
{
    public UnityEvent m_Acumilated;
    public UnityEvent m_UnAcumilated;


    [SerializeField] private int m_ExpectedInputs;
    private int m_currentInputs = 0;
    private bool m_active = false;

    public void AddTo()
    {
        m_currentInputs++;
        if (m_currentInputs >= m_ExpectedInputs && !m_active)
        {
            m_Acumilated?.Invoke();
            m_active = true;
        }
    }

    public void RemoveTo()
    {
        m_currentInputs--;
        if (m_currentInputs < m_ExpectedInputs && m_active)
        {
            m_active = false;
            m_UnAcumilated?.Invoke();
        }
    }
}
