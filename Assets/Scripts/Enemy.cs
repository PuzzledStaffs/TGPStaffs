using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
  
    int m_health = 50;
    public StateManager manager;
    public FOV fieldOfView;

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
       // manager.ChangeState(State.StateType.IDLE);
    }

    // Update is called once per frame
    void Update()
    {

        if (fieldOfView.inFOV)
        {
            Debug.Log("Player");
            manager.ChangeState(State.StateType.ATTACK);
        }
        else
        { 
        
            
        }
        
    }

    IEnumerator DeathCoroutine()
    {
        //TODO: Add death animation
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
