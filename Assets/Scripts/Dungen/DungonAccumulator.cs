using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;

public class DungonAccumulator : MonoBehaviour
{
    public UnityEvent m_Acumilated;
    public UnityEvent m_UnAcumilated;


    [SerializeField] private int m_ExpectedInputs;
    [SerializeField] private TextMeshProUGUI m_Text;
    private int m_currentInputs = 0;
    private bool m_active = false;

    private void Start()
    {
        m_Text.text = m_currentInputs.ToString();
        m_Text.color = Color.red;
    }


    public void AddTo()
    {
        m_currentInputs++;
        m_Text.text = m_currentInputs.ToString();
        
        if (m_currentInputs >= m_ExpectedInputs && !m_active)
        {
            m_Acumilated?.Invoke();
            m_active = true;
            m_Text.color = Color.green;
        }
    }

    public void RemoveTo()
    {
        m_currentInputs--;
        m_Text.text = m_currentInputs.ToString();
        if (m_currentInputs < m_ExpectedInputs && m_active)
        {
            m_active = false;
            m_UnAcumilated?.Invoke();
            m_Text.color = Color.red;
        }
    }
}
