using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    int m_health = 50;

    public int GetHealth()
    {
        return m_health;
    }

    public bool isDead(int health)
    {
        if (health <= 0)
        {
            return true;
        }
        else 
        {
            return false;

        }


    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;

        if (isDead(m_health))
        {
            StartCoroutine(DeathCoroutine());
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //update state
    }

    IEnumerator DeathCoroutine()
    {
        //TODO: Add death animation
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }



}
