using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    bool phaseSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        m_health = 400;
        manager = new BossStateManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health <= 200)
        {
            phaseSwitch = true;
        }

        //Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);

        if (fieldOfView.inFOV)
        {
            if (phaseSwitch)
            {
                manager.ChangeState(State.StateType.R_ATTACK);
            }
            else 
            {
                manager.ChangeState(State.StateType.ATTACK);
            }
        }
    }
}
