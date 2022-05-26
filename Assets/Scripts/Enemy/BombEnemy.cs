using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : RangedEnemy
{

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        ChangeState(StateType.FLEE);
    }

    public override void ChangeState(StateType state)
    {
        switch (state)
        {
            case StateType.IDLE:
                m_currentState = StateType.IDLE;
                IdleState();
                break;
            case StateType.CHASE:
                m_currentState = StateType.CHASE;
                ChasePlayer();
                break;
            case StateType.FLEE:
                m_currentState = StateType.FLEE;
                Flee();
                break;
            default:
                break;
        }

    }

    void Flee()
    {
        if (m_cooldown >= 0)
        {
            Vector3 dir = (m_pathToPlayer.corners[1] - transform.position).normalized;
            dir = dir * -1;
            var rotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
            m_rb.velocity = (Vector3.ClampMagnitude(m_rb.velocity, 3f));
            m_rb.AddForce(dir * m_speed);
        }
    }
}
