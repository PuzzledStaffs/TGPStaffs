using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    bool phaseSwitch = false;
    int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        m_health = 100;
        maxHealth = m_health;
        m_fieldOfView = GetComponent<FOV>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health <= maxHealth/2)
        {
            phaseSwitch = true;
        }

        //Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        //animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);


        if (m_fieldOfView.inFOV == true)
        {
            if (phaseSwitch)
            {
                Debug.Log("ranged");
                m_manager.ChangeState(State.StateType.R_ATTACK);
            }
            else 
            {
                Debug.Log("melee");
                m_manager.ChangeState(State.StateType.ATTACK);
            }
        }
    }

    public override void TakeDamage(IHealth.Damage damage)
    {
        if (IsDead())
            return;
        if (phaseSwitch && damage.type == IHealth.DamageType.BOW)
        {

        }
        else
        {
            m_health -= damage.damageAmount;
            m_animator.SetTrigger("TakeDamage");
        }

        if (IsDead())
        {
            StartCoroutine(DeathCoroutine());
        }

    }
}
