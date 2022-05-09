using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : StateManager
{
    public State rangedAttack;

    private void Start()
    {
        
    }

    public override void ChangeState(State.StateType type)
    {
        switch (type)
        {
            case State.StateType.IDLE:
                m_roam.enabled = false;
                m_attack.enabled = false;
                m_idle.enabled = true;
                rangedAttack.enabled = false;
                break;
            case State.StateType.ROAM:
                m_roam.enabled = true;
                m_attack.enabled = false;
                m_idle.enabled = false;
                rangedAttack.enabled = false;
                break;
            case State.StateType.ATTACK:
                m_roam.enabled = false;
                m_attack.enabled = true;
                m_idle.enabled = false;
                rangedAttack.enabled = false;
                break;
            case State.StateType.R_ATTACK:
                m_roam.enabled = false;
                m_attack.enabled = false;
                m_idle.enabled = false;
                rangedAttack.enabled = true;
                break;
        }
    }
}
