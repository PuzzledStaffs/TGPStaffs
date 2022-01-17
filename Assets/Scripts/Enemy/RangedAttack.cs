using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackState
{
    public int range;
    public GameObject projectile;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to player
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookTowards = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, Time.deltaTime * 0.5f);

        //Stops the enemy from moving
        agent.isStopped = true;


        //intansiate projectile
        //projectile.Addrelativeforce (rigidbody)

    }

}
