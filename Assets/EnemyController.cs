using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHealth
{
    [Header("---------------Enemy Statistics------------------------------------------------------------")]
    public int m_health = 50;
    public float m_speed;
    public float m_attackCooldown;
    private bool m_canAttack = true;
    [Header("---------------Generic------------------------------------------------------------")]
    public NavMeshAgent m_agent;
    Transform m_player;
    private NavMeshPath m_pathToPlayer;
    public Rigidbody m_rb;
    public float m_pushBackForce;
    public float m_deathForce;
    public GameObject m_destroyParticle;


    public Vector3 m_offset;
    public float m_dodgeChance = 0;
    private bool m_takingDamage = false;
    public int m_currentState = 0;

    public LayerMask m_whatIsPlayer, m_whatIsGround;

    public bool m_died = false;
    [Header("---------------States------------------------------------------------------------")]
    public float m_sightRange, m_attackRange;
    public bool m_playerInSightRange, m_playerInAttackRange;


    private void Start()
    {
        m_pathToPlayer = new NavMeshPath();
        InvokeRepeating("CalculatePath", 0.5f, 0.5f);
    }

    private void Awake()
    {
        m_player = GameObject.Find("Player").transform;
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

        //Take Damage - check for collision

       


    }
    void CalculatePath()
    {
        if (m_currentState == 2)
        {
            m_agent.Warp(transform.position);
            m_pathToPlayer = new NavMeshPath();
            m_agent.CalculatePath(m_player.position, m_pathToPlayer);
        }
    }

    void ChasePlayer()
    {
        if (m_pathToPlayer.corners.Length > 1 && !m_died && m_currentState != 4)
        {
            Debug.Log("Chase State");
            m_currentState = 2;
            if (!m_takingDamage || m_currentState != 3) // walking
            {
                
            }
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
            

            if (m_canAttack)
            {
                m_rb.velocity = new Vector3(0, 0, 0);
                m_currentState = 3;
                transform.LookAt(m_player);
                StartCoroutine(AttackCoroutine());
            }
            
            //not walking
        }

    }

    IEnumerator AttackCoroutine()
    {
        m_canAttack = false;
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1.0f, 1.0f, 1.0f), transform.rotation);
        foreach (var hitCollider in colliders)
        {
            //if its the player, then take damage
            //Make sure player object tag is set to "Player"
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Attacking Player");

                IHealth.Damage damageStruct = new IHealth.Damage();
                damageStruct.damageAmount = 1;
                damageStruct.type = IHealth.DamageType.ENEMY;

                //Take Damage
                IHealth health = m_player.GetComponent<IHealth>();
                health.TakeDamage(damageStruct);
            }
        }
        yield return new WaitForSeconds(m_attackCooldown);
        m_canAttack = true;
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
        m_currentState = 4;
        m_rb.velocity = new Vector3(0, 0, 0);
        //yield return new WaitForSeconds(1.8f);
        yield return new WaitForSeconds(1f);
        m_currentState = 2;
        m_takingDamage = false;
    }

    public void Dodge()
    {
        if (m_currentState != 4) //if not equal to take damage state
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
        m_health -= damage.damageAmount;

        m_currentState = 4; //sets the state to take damage
        m_takingDamage = true;
        //animator.SetBool("Walking", false);
        m_rb.AddForce(-transform.forward * m_pushBackForce);
        StartCoroutine(StopMoving());
        m_canAttack = false;    
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
