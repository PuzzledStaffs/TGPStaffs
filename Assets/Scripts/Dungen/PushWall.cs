using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PushWall : Trap
{

    [SerializeField] float m_wallDelay;
    [SerializeField] bool m_pushOnActivate;
    Coroutine wait;
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public override void EnterRoomEnabled()
    {
        if (m_pushOnActivate)
            m_animator.SetTrigger("Push");
    }

    public override void ExitRoomDisabled()
    {
        //End push pull sequence

        //Reset position
    }

    public void WallCycleFinished()
    {

    }
}
