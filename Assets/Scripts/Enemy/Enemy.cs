using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField]
    protected int m_health = 20;
    public StateManager m_manager;
    public FOV m_fieldOfView;
    public Animator m_animator;
    public Action<GameObject> m_deadEvent;
    [SerializeField] GameObject m_DeathDrop;
    [SerializeField] RectTransform m_healthBar;
    [SerializeField] RectTransform m_healthBarMask;

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
        m_animator.SetTrigger("TakeDamage");

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

        m_healthBar.position = transform.position + Vector3.forward;
        Vector3 lookPos = Camera.main.transform.position - m_healthBar.position;
        lookPos.x = 0;
        lookPos.z = 0;
        m_healthBar.rotation = Quaternion.LookRotation(lookPos);

        //Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        m_animator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if (m_fieldOfView.inFOV)
        {
            m_manager.ChangeState(State.StateType.ATTACK);
        }      
    }

    public IEnumerator DeathCoroutine()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<NavMeshAgent>().speed = 0;
        this.GetComponent<BoxCollider>().enabled = false;
        RandomDeathAnim();
        m_deadEvent?.Invoke(gameObject);
        m_animator.SetBool("Dead", true);
        
        yield return new WaitForSeconds(2.6f);
        if(m_DeathDrop != null)
            Instantiate(m_DeathDrop, transform.position,transform.rotation,transform.parent);
        Destroy(this.gameObject);
        
    }

    void RandomDeathAnim()
    {
        int random = UnityEngine.Random.Range(0, 2);
        m_animator.SetInteger("DeathAnim", random);
        Debug.Log(random);
    }

    IEnumerator TakeDamageCoroutine()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        yield return new WaitForSeconds(1f);
        GetComponent<NavMeshAgent>().isStopped = false;
    }
}
