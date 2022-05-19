using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : Trap
{

    public override void EnterRoomEnabled()
    {

    }
    public override void ExitRoomDisabled()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IHealth.Damage damage;
        damage.damageAmount = 1;
        damage.type = IHealth.DamageType.ENVIRONMENT;

        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerController>()?.Respawn();
            other.transform.GetComponent<IHealth>()?.TakeDamage(damage);
        }
        else
        {
            damage.damageAmount = 10000;
            other.transform.GetComponent<IHealth>()?.TakeDamage(damage);
        }
    }
}
