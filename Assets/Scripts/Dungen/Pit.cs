using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IHealth.Damage damage;
        damage.damageAmount = 1;
        damage.type = IHealth.DamageType.ENVIRONMENT;
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerController>().Respawn();
            other.transform.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
