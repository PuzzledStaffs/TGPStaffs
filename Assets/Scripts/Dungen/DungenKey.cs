using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungenKey : Trap
{

    [FormerlySerializedAs("m_dungenManager")]  private DungenManager m_dungeonManager;
    private Animator m_animator;
    public AudioClip m_pickupSound;

    private void Awake()
    {
        m_dungeonManager = FindObjectOfType<DungenManager>();
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
            other.GetComponent<PlayerController>().m_audioSource.PlayOneShot(m_pickupSound);
            m_dungeonManager.AddKey();
            gameObject.SetActive(false);
        }
    }
}
