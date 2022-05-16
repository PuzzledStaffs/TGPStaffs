using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : State, IHealth
{
    [Header("---------------Enemy Statistics------------------------------------------------------------")]
    public int m_health = 50;
    public float m_speed;
    [Header("---------------Generic------------------------------------------------------------")]
    public NavMeshAgent m_agent;
    Transform m_player;
    private NavMeshPath m_pathToPlayer;
    public Rigidbody m_rb;
    public float m_pushBackForce;
    public float m_deathForce;
    public GameObject m_destroyParticle;
    public Animator m_animator;

    public Vector3 m_offset;
    public float m_dodgeChance = 0;
    private bool m_takingDamage = false;
   // public int m_currentState = 0;
    public StateType m_stateType;

    public LayerMask m_whatIsPlayer, m_whatIsGround;

    public bool m_died = false;
    [Header("---------------States------------------------------------------------------------")]
    public float m_sightRange, m_attackRange;
    public bool m_playerInSightRange, m_playerInAttackRange;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_stateType = StateType.CHASE;
        m_pathToPlayer = new NavMeshPath();
        Debug.Log("calculate 1");
        InvokeRepeating("CalculatePath", 0.5f, 0.5f);
    }

    private void Awake()
    {       
        m_agent = GetComponent<NavMeshAgent>();
        // animator = GetComponent<Animator>();
        m_agent.updatePosition = false;
        m_agent.updateRotation = false;
    }

    private void Update()
    {
        //Chekc for sight and attack range
        m_playerInSightRange = Physics.CheckSphere(transform.position, m_sightRange, m_whatIsPlayer);
        m_playerInAttackRange = Physics.CheckSphere(transform.position, m_attackRange, m_whatIsPlayer);

        if (!m_takingDamage)
        {
            if (m_playerInSightRange && !m_playerInAttackRange) ChasePlayer();
            if (m_playerInSightRange && m_playerInAttackRange) AttackPlayer();
        }

    }
    void CalculatePath()
    {
        Debug.Log("calculate 2");
        if(m_stateType == StateType.CHASE)//if (m_currentState == 2)
        {
            Debug.Log("calculate 3");
            m_agent.Warp(transform.position);
            m_pathToPlayer = new NavMeshPath();
            m_agent.CalculatePath(m_player.position, m_pathToPlayer);
        }
    }

    void ChasePlayer()
    {
        //if (m_pathToPlayer.corners.Length > 1 && !m_died && m_currentState != 4)
        if (m_pathToPlayer.corners.Length > 1 && !m_died && m_stateType != StateType.IDLE)
        {
            Debug.Log("Chase State");
           // m_currentState = 2;
            m_stateType = StateType.CHASE;
          //  if (!m_takingDamage || m_currentState != 3) // walking
         //   {
                
        //    }
            //Apply Force in the direction fo the next point
            Vector3 dir = (m_pathToPlayer.corners[1] - transform.position).normalized;
            var rotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);           
            m_rb.velocity = (Vector3.ClampMagnitude(m_rb.velocity, 3f));
            m_rb.AddForce(dir * m_speed);
            if (Vector3.Distance(transform.position, new Vector3(m_pathToPlayer.corners[1].x, transform.position.y, m_pathToPlayer.corners[1].z)) < 6)
            {
                CalculatePath();
            }
        }
    }

    void AttackPlayer()
    {
        if (!m_died)
        {
            m_rb.velocity = new Vector3(0, 0, 0);
           // m_currentState = 3;
            m_stateType = StateType.ATTACK;
            transform.LookAt(m_player);
            //not walking
        }

    }
    void death()
    {
        m_died = true;
        StartCoroutine(DestroyObject());
        //animator.SetTrigger("Death");
        m_rb.AddForce(-transform.forward * m_deathForce);
        m_rb.AddForce(transform.up * m_deathForce);
        // GameManager.gameObject.GetComponent<TimeManager>().SlowDownTime();
    }

    IEnumerator DestroyObject()
    {
        Instantiate(m_destroyParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);       
        Destroy(gameObject);
    }

    IEnumerator StopMoving()
    {
      //  m_currentState = 4;
        m_stateType = StateType.IDLE;
        m_rb.velocity = new Vector3(0, 0, 0);
        //yield return new WaitForSeconds(1.8f);
        yield return new WaitForSeconds(1f);
     //   m_currentState = 2;
        m_stateType = StateType.CHASE;
        m_takingDamage = false;
    }

    public void Dodge()
    {
        if(m_stateType != StateType.IDLE)
        //if (m_currentState != 4) //if not equal to take damage state
        {
            m_dodgeChance += 1 * Time.deltaTime;
            float randomNum = Random.Range(0, 1000 - (m_dodgeChance * 200));
            if (randomNum <= 1)
            {
                Debug.Log("Dodged with a percent of: " + m_dodgeChance * 10);
                m_dodgeChance = 0;
                gameObject.GetComponent<Animator>().SetTrigger("Dodge");
                StartCoroutine(StopMoving());
            }
        }
    }

    public int GetHealth()
    {
        return m_health;
    }

    public void TakeDamage(IHealth.Damage damage)
    {
        if(damage.type == IHealth.DamageType.SWORD)
        {
            StartCoroutine(TakeDamageWait(damage, 0.5f));
        }
        else
        {
            StartCoroutine(TakeDamageWait(damage, 0.0f));
        }
        
    }

    IEnumerator TakeDamageWait(IHealth.Damage damage, float time)
    {
        yield return new WaitForSeconds(time);
        m_health -= damage.damageAmount;
        m_animator.SetTrigger("EnemyHit");
      //  m_currentState = 4; //sets the state to take damage
        m_stateType = StateType.IDLE;
        m_takingDamage = true;
        //animator.SetBool("Walking", false);
        m_rb.AddForce(-transform.forward * m_pushBackForce);
        StartCoroutine(StopMoving());
        // animator.SetTrigger("Hit");
        if (IsDead())
        {
            death();
        }
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
}
