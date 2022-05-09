using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField]
    protected int m_health = 20;
    public StateManager manager;
    public FOV fieldOfView;
    public Animator animator;
    public Action<GameObject> m_deadEvent;
    public RectTransform m_healthBar;
    public RectTransform m_healthBarMask;

    public int GetHealth()
    {
        return m_health;
    }

    public bool IsDead()
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

    public virtual void TakeDamage(IHealth.Damage damage)
    {
        if (IsDead()) { return; }

        

        m_health -= damage.damageAmount;
        m_healthBarMask.sizeDelta = new Vector2(4.5f * (m_health / 20.0f), 0.5f);
        animator.SetTrigger("TakeDamage");

        if (IsDead())
        {
            StartCoroutine(DeathCoroutine());
        }
        StartCoroutine(TakeDamageCoroutine());

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (IsDead())
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().speed = 0;
        }

        m_healthBar.position = transform.position + Vector3.forward;
        var lookPos = Camera.main.transform.position - m_healthBar.position;
        lookPos.x = 0;
        lookPos.z = 0;
        m_healthBar.rotation = Quaternion.LookRotation(lookPos);

        //Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if (fieldOfView.inFOV)
        {
            manager.ChangeState(State.StateType.ATTACK);
        }      
    }

    public IEnumerator DeathCoroutine()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        RandomDeathAnim();
        m_deadEvent?.Invoke(gameObject);
        animator.SetBool("Dead", true);
        
        yield return new WaitForSeconds(2.6f);
        Destroy(this.gameObject);

    }

    void RandomDeathAnim()
    {
        int random = UnityEngine.Random.Range(0, 2);
        animator.SetInteger("DeathAnim", random);
        Debug.Log(random);
    }

    IEnumerator TakeDamageCoroutine()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        yield return new WaitForSeconds(1f);
        GetComponent<NavMeshAgent>().isStopped = false;
    }
}
