using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject player;
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
            //if it hits the player, take damage
            player.GetComponent<PlayerController>().TakeDamage(attack.damage);
            Debug.Log("Hit player");

            Destroy(this.gameObject);
        }
    }
}
