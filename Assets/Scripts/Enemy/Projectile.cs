using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float m_timer = 0;
    public int m_damageAmount;
    Rigidbody rb;
    public float velocity;
    public GameObject attack;
    Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = attack.transform.forward;
    }
    // Start is called before the first frame update
    void Update()
    {
        m_timer += Time.deltaTime;

        rb.AddForce(moveDirection * velocity, ForceMode.Force);
        if (m_timer >= 3)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            IHealth.Damage damage = new IHealth.Damage();
            damage.damageAmount = m_damageAmount;
            damage.type = IHealth.DamageType.ENEMY;

            //if it hits the player, take damage
           collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
    }
}
