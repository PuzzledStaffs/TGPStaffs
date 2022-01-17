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
      //  player = GetComponentInParent<FOV>().target;
      //  attack = GetComponentInParent<RangedAttack>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if (collision.collider.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().TakeDamage(attack.damage);
            Debug.Log("Hit player");

            Destroy(this.gameObject);
        }
    }
}
