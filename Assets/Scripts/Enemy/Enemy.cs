using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
  
    int m_health = 20;
    public StateManager manager;
    public FOV fieldOfView;

    public int GetHealth()
    {
        return m_health;
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

    public void TakeDamage(int damage)
    {
        if (isDead())
            return;

        Debug.Log("Ouch");
        m_health -= damage;

        if (isDead())
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

        if (fieldOfView.inFOV)
        {
            manager.ChangeState(State.StateType.ATTACK);
        }      
    }

    IEnumerator DeathCoroutine()
    {
        //TODO: Add death animation
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
