using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    void Start()
    {
        m_health = 30;
        
    }


    override public void AttackPlayer()
    {
        if (!m_died && m_canAttack)
        {
            m_canAttack = false;
            m_rb.velocity = new Vector3(0, 0, 0);
            m_currentState = StateType.ATTACK;
            transform.LookAt(m_player);

            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1.0f, 1.0f, 1.0f), transform.rotation);
            foreach (var hitCollider in colliders)
            {
                //if its the player, then take damage
                //Make sure player object tag is set to "Player"
                if (hitCollider.CompareTag("Player"))
                {
                    Debug.Log("Attacking Player");

                    IHealth.Damage damageStruct = new IHealth.Damage();
                    damageStruct.damageAmount = m_damage;
                    damageStruct.type = IHealth.DamageType.ENEMY;

                    //Take Damage
                    IHealth health = m_player.GetComponent<IHealth>();
                    health.TakeDamage(damageStruct);
                    m_player.GetComponent<PlayerController>().PlayerKnockBack(gameObject);


                }
            }

            StartCoroutine(AttackCooldown());
        }
    }

}
