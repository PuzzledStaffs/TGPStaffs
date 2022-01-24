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
                roam.enabled = false;
                attack.enabled = false;
                idle.enabled = true;
                rangedAttack.enabled = false;
                break;
            case State.StateType.ROAM:
                roam.enabled = true;
                attack.enabled = false;
                idle.enabled = false;
                rangedAttack.enabled = false;
                break;
            case State.StateType.ATTACK:
                roam.enabled = false;
                attack.enabled = true;
                idle.enabled = false;
                rangedAttack.enabled = false;
                break;
            case State.StateType.R_ATTACK:
                roam.enabled = false;
                attack.enabled = false;
                idle.enabled = false;
                rangedAttack.enabled = true;
                break;
        }
    }
}
