using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PushWall : Trap
{

    [SerializeField] float m_wallDelay;
    [SerializeField] bool m_pushOnActivate;
    Coroutine m_waitCorutine;
    Animator m_animator;
    bool m_active;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_active = false;
    }

    public override void EnterRoomEnabled()
    {
        m_active = true;
        m_animator.enabled = true;

        if (m_pushOnActivate)
            m_animator.SetTrigger("Push");
        else
            m_waitCorutine = StartCoroutine(CWallWait());
    }

    public override void ExitRoomDisabled()
    {
        //End push pull sequence
        StopCoroutine(m_waitCorutine);
        m_active = false;
        m_animator.enabled = false;
    }

    public void WallCycleFinished()
    {
        if(m_active)
            m_waitCorutine = StartCoroutine(CWallWait());
    }

    IEnumerator CWallWait()
    {
        yield return new WaitForSecondsRealtime(m_wallDelay + 0.5f);
        m_animator.SetTrigger("Push");
    }
}
