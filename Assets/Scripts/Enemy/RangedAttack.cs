using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackState
{
    public int m_range;
    public GameObject m_projectile;
    public GameObject m_model;
    public float m_velocity;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
       
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        //rotate to player
        Vector3 direction = (player.transform.position - transform.position).normalized;
         Quaternion lookTowards = Quaternion.LookRotation(direction);
         transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, Time.deltaTime * 5.0f);

        //Stops the enemy from moving
        agent.isStopped = true;
        if (cooldown <= 0)
        {
            cooldown = maxCooldown;
            GameObject attack = Instantiate(m_projectile, transform.position + new Vector3(0, 1.0f,0), transform.rotation);

            attack.GetComponent<Projectile>().m_damageAmount = damage;
            attack.GetComponent<Projectile>().m_velocity = m_velocity;
            attack.GetComponent<Projectile>().m_attack = m_model;
        }
    }

}
