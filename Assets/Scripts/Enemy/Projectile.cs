using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject player;
    public RangedAttack attack;
    IHealth.Damage damage;

    // Start is called before the first frame update
    void Start()
    {
        //Dont work
      //  player = GetComponentInParent<FOV>().target;
      //  attack = GetComponentInParent<RangedAttack>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            damage.damageAmount = attack.damage;
            damage.type = IHealth.DamageType.ENEMY;
            //if it hits the player, take damage
            player.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
    }
}
