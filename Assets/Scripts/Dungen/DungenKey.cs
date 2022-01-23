using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenKey : Trap
{

   private DungenManager m_dungenManager;
   private Animator m_animator;

    private void Awake()
    {
        m_dungenManager = FindObjectOfType<DungenManager>();
        m_animator = GetComponent<Animator>();
    }

    public override void EnterRoomEnabled()
    {
        m_animator.enabled = true;
    }
    public override void ExitRoomDisabled()
    {
        m_animator.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_dungenManager.AddKey();
            gameObject.SetActive(false);
        }
    }
}
