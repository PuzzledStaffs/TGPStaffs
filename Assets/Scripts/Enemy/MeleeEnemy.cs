using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    public Vector3 m_colliderRange;

    override public void AttackPlayer()
    {
        if (!m_died && m_canAttack && !m_firstTime)
        {
            m_canAttack = false;
            m_rb.velocity = new Vector3(0, 0, 0);
            m_currentState = StateType.ATTACK;
            transform.LookAt(m_player);

            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, m_colliderRange, transform.rotation);
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
        else if (!m_died && m_canAttack && m_firstTime)
        {
            StartCoroutine(FirstTimeCooldown());
        }
    }

    IEnumerator FirstTimeCooldown()
    {
        yield return new WaitForSeconds(2.0f);
        m_firstTime = false;
    }
}
