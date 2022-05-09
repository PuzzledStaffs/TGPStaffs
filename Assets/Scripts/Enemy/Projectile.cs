using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float m_timer = 0;
    public int m_damageAmount;
    Rigidbody m_rigidBody;
    public float m_velocity;
    public GameObject m_attack;
    Vector3 m_moveDirection;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_moveDirection = m_attack.transform.forward;
    }
    // Start is called before the first frame update
    void Update()
    {
        m_timer += Time.deltaTime;

        m_rigidBody.AddForce(m_moveDirection * m_velocity, ForceMode.Force);
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
