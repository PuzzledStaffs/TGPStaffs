using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldBridge : MonoBehaviour
{
    [SerializeField] private bool m_open;
    [SerializeField] private float m_closedXRotation;
    [SerializeField] private float m_openXRotation;


    void Start()
    {
        if(m_open)
            OpenBridge();
        else
            CloseBridge();
    }

    public void OpenBridge()
    {
        transform.rotation = Quaternion.Euler(m_openXRotation, 0, 0);
        m_open = true;
    }

    public void CloseBridge()
    {
        transform.rotation = Quaternion.Euler(m_closedXRotation, 0, 0);
        m_open = false;

    }
}
