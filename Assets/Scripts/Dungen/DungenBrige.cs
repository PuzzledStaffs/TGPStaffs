using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DungenBrige : MonoBehaviour
{
    
    [SerializeField] [FormerlySerializedAs("m_Collider")] private BoxCollider m_collider;
    [SerializeField] private bool m_open;
    [SerializeField][FormerlySerializedAs("m_brige")] private GameObject m_bridge;
    
    
    private Animator m_animation;

    private void Start()
    {
        m_animation = GetComponent<Animator>();
        if (m_open)
        {
            m_animation.SetTrigger("Open");
            m_collider.enabled = true;
        }
        else
        {
            m_animation.SetTrigger("Closed");
            m_collider.enabled = false;
        }
    }
    public void OnOpen()
    {
        m_collider.enabled = true;
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
        m_collider.enabled = false;
    }

    public void ShowBrige()
    {
        m_bridge.SetActive(true);
    }

    public void HideBrige()
    {
        m_bridge.SetActive(false);
    }
}
