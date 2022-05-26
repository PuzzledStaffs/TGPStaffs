using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 m_desiredVelocity;
    public int m_damageAmount;
    Rigidbody m_rigidBody;
    public float m_velocity;
    public GameObject m_attack;
    Vector3 m_moveDirection;
    public Vector3 m_target;
 

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_moveDirection = m_attack.transform.forward;
        m_desiredVelocity = ((m_target + new Vector3(0, 1.0f, 0)) - transform.position).normalized * m_velocity;

    }
    // Start is called before the first frame update
    void FixedUpdate()
    {
        m_rigidBody.velocity = m_desiredVelocity;
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
