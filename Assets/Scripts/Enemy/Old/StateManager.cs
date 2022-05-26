using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Enemy m_enemy;
    public IdleState m_idle;
    public AttackState m_attack;
    public RoamingState m_roam;

    public virtual void ChangeState(State.StateType type)
    {
        switch (type)
        {
            case State.StateType.IDLE:
                m_roam.enabled = false;
                m_attack.enabled = false;
                m_idle.enabled = true;
                break;
            case State.StateType.ROAM:
                m_roam.enabled = true;
                m_attack.enabled = false;
                m_idle.enabled = false;
                break;
            case State.StateType.ATTACK:
                m_roam.enabled = false;
                m_attack.enabled = true;
                m_idle.enabled = false;
                break;
        }
    }
}
