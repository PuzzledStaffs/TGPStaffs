using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungenBrige : MonoBehaviour
{
    
   
    [SerializeField] private BoxCollider m_Collider;
    [SerializeField] private bool m_open;
    [SerializeField] private GameObject m_brige;
    
    
    private Animator m_animation;

    private void Start()
    {
        m_animation = GetComponent<Animator>();
        if (m_open)
        {
            m_animation.SetTrigger("Open");
            m_Collider.enabled = true;
        }
        else
        {
            m_animation.SetTrigger("Closed");
            m_Collider.enabled = false;
        }
    }
    public void OnOpen()
    {
        m_Collider.enabled = true;
        m_open = true;
        m_animation.SetTrigger("Open");

    }

    public void OnClose()
    {
        m_open = false;
        m_animation.SetTrigger("Closed");
    }

    public void FinishClose()
    {
        m_Collider.enabled = false;
    }

    public void ShowBrige()
    {
        m_brige.SetActive(true);
    }

    public void HideBrige()
    {
        m_brige.SetActive(false);
    }
}
