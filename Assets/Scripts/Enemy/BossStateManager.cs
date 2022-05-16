using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossStateManager : StateManager
{
    [FormerlySerializedAs("rangedAttack")]
    public State m_rangedAttack;

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
                m_rangedAttack.enabled = false;
                break;
            case State.StateType.ROAM:
                m_roam.enabled = true;
                m_attack.enabled = false;
                m_idle.enabled = false;
                m_rangedAttack.enabled = false;
                break;
            case State.StateType.ATTACK:
                m_roam.enabled = false;
                m_attack.enabled = true;
                m_idle.enabled = false;
                m_rangedAttack.enabled = false;
                break;
                //    case State.StateType.R_ATTACK:
                //        m_roam.enabled = false;
                //        m_attack.enabled = false;
                //        m_idle.enabled = false;
                //        m_rangedAttack.enabled = true;
                //        break;
        }
    }
}
