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
        fieldOfView = GetComponent<FOV>();
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


        if (fieldOfView.inFOV == true)
        {
            if (phaseSwitch)
            {
                Debug.Log("ranged");
                manager.ChangeState(State.StateType.R_ATTACK);
            }
            else 
            {
                Debug.Log("melee");
                manager.ChangeState(State.StateType.ATTACK);
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
            animator.SetTrigger("TakeDamage");
        }

        if (IsDead())
        {
            StartCoroutine(DeathCoroutine());
        }

    }
}
