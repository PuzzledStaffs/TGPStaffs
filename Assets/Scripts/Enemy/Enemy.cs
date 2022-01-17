using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
  
    int m_health = 20;
    public StateManager manager;
    public FOV fieldOfView;
    public Animator animator;

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
        animator.SetTrigger("TakeDamage");

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
        Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if (fieldOfView.inFOV)
        {
            manager.ChangeState(State.StateType.ATTACK);
        }      
    }

    IEnumerator DeathCoroutine()
    {
        //TODO: Add death animation
        RandomDeathAnim();
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2.6f);
        Destroy(gameObject);

    }

    void RandomDeathAnim()
    {
        int random = Random.Range(0, 2);
        animator.SetInteger("DeathAnim", random);
        Debug.Log(random);
    }
}
