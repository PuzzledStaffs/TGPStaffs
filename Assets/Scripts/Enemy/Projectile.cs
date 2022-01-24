using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public RangedAttack attack;

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
            IHealth.Damage damage = new IHealth.Damage();
            damage.damageAmount = attack.damage;
            damage.type = IHealth.DamageType.ENEMY;
            //if it hits the player, take damage
            attack.player.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
    }
}
