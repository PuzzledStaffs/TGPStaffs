using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Serialization;

public class DungonAccumulator : MonoBehaviour
{
    [FormerlySerializedAs("m_Acumilated")]
    public UnityEvent m_accumulated;
    [FormerlySerializedAs("m_UnAcumilated")]
    public UnityEvent m_unAccumulated;

    [FormerlySerializedAs("m_ExpectedInputs")]
    [SerializeField] private int m_expectedInputs;
    [FormerlySerializedAs("m_Text")]
    [SerializeField] private TextMeshProUGUI m_text;

    private int m_currentInputs = 0;
    private bool m_active = false;

    private void Start()
    {
        m_text.text = m_currentInputs.ToString();
        m_text.color = Color.red;
    }


    public void AddTo()
    {
        m_currentInputs++;
        m_text.text = m_currentInputs.ToString();
        
        if (m_currentInputs >= m_expectedInputs && !m_active)
        {
            m_accumulated?.Invoke();
            m_active = true;
            m_text.color = Color.green;
        }
    }

    public void RemoveTo()
    {
        m_currentInputs--;
        if(m_currentInputs < 0)
        {
            m_currentInputs = 0;
        }

        m_text.text = m_currentInputs.ToString();
        if (m_currentInputs < m_expectedInputs && m_active)
        {
            m_active = false;
            m_unAccumulated?.Invoke();
            m_text.color = Color.red;
        }
    }
}
