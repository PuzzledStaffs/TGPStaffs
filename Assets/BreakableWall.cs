using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IHealth
{
    int m_health = 999;

    public int GetHealth()
    {
        return m_health;
    }
    public void TakeDamage(int damage)
    {
        m_health -= damage;
    }
    public bool isDead()
    {
        if (m_health <= 0)
        {

            return true;
        }
        else
        {
            return false;

        }
    }
    void Update()
    {
        if (isDead())
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
