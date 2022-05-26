using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class RangedEnemy : EnemyController
{
    public GameObject m_projectile;
    public GameObject m_model;
    public float m_cooldown;

    public virtual void AttackPlayer()
    {
        //rotate to player
        Vector3 direction = (m_player.transform.position - transform.position).normalized;
        Quaternion lookTowards = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, Time.deltaTime * 5.0f);

        if (m_cooldown <= 0)
        {
            StartCoroutine(AttackCooldown());
            m_cooldown = m_attack_cooldown;
            GameObject attack = Instantiate(m_projectile, transform.position + new Vector3(0, 3.5f, 0.5f), transform.rotation);

            attack.GetComponent<Projectile>().m_target = GameObject.FindGameObjectWithTag("Player").transform.position;
            attack.GetComponent<Projectile>().m_damageAmount = m_damage;
            attack.GetComponent<Projectile>().m_attack = m_model;
            
        }
    }

    protected override void Update()
    {
        base.Update();
        m_cooldown -= Time.deltaTime;
    }
}
