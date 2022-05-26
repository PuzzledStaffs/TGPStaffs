using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyController
{
    public GameObject m_projectile;
    public GameObject m_model;
    public GameObject m_bomb;
    public GameObject m_bombModel;
    public float m_cooldown;



    public override void AttackPlayer()
    {
        if (m_health <= 50 && m_health > 40 || m_health <= 20 && m_health > 10)
        {
            AttackMelee();
        }
        else if(m_health <= 40 && m_health > 20 || m_health <= 10 && m_health > 0)
        {
            AttackRanged();
        }
    }

    protected override void Update()
    {
        base.Update();
        m_cooldown -= Time.deltaTime;
    }


    void AttackMelee()
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

    void AttackRanged()
    {
        //rotate to player
        //Vector3 direction = (m_player.transform.position - transform.position).normalized;
        Quaternion lookTowards = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, Time.deltaTime * 5.0f);
        if (m_cooldown <= 0)
        {
            StartCoroutine(RangedAttackCooldown());
            m_cooldown = m_attack_cooldown;

            if (m_health <= 10)
            {
                GameObject attack = Instantiate(m_bomb, transform.position + new Vector3(0, 3.5f, 0.5f), transform.rotation);
                attack.GetComponent<Projectile>().m_target = GameObject.FindGameObjectWithTag("Player").transform.position;
                attack.GetComponent<Projectile>().m_damageAmount = m_damage;
                attack.GetComponent<Projectile>().m_attack = m_bombModel;
            }
            else 
            {
                GameObject attack = Instantiate(m_projectile, transform.position + new Vector3(0, 3.5f, 0.5f), transform.rotation);
                attack.GetComponent<Projectile>().m_target = GameObject.FindGameObjectWithTag("Player").transform.position;
                attack.GetComponent<Projectile>().m_damageAmount = m_damage;
                attack.GetComponent<Projectile>().m_attack = m_model;
            }
        }
    }

    protected IEnumerator RangedAttackCooldown()
    {
        m_animator.SetTrigger("RangedAttack");
        yield return new WaitForSeconds(m_attack_cooldown);
        m_canAttack = true;

    }


}
