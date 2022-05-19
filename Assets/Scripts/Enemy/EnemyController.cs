
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
    bool m_canAttack = true;
    public Vector3 m_offset;
    public float m_dodgeChance = 0;
    private bool m_takingDamage = false;
    public System.Action<GameObject> m_deadEvent;
    [FormerlySerializedAs("m_DeathDrop")]
    [SerializeField] GameObject m_deathDrop;

    [FormerlySerializedAs ("m_stateType")]
    public StateType m_currentState;
    public int m_damage;

    public float m_attack_cooldown;
    public LayerMask m_whatIsPlayer, m_whatIsGround;

    public bool m_died = false;
    public bool m_deleteSelf = false;
    [Header("---------------States------------------------------------------------------------")]
    public float m_sightRange, m_attackRange;
    public bool m_playerInSightRange, m_playerInAttackRange;

    public GameObject m_attackParticle;

    private void Start()
    {
        if (m_deleteSelf)
            Destroy(gameObject);
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_currentState = StateType.CHASE;
        m_pathToPlayer = new NavMeshPath();
        InvokeRepeating("CalculatePath", 0.5f, 0.5f);
    }

    private void Awake()
    {       
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updatePosition = false;
        m_agent.updateRotation = false;
    }

    private void Update()
    {
        //Check for sight and attack range
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
        if(m_currentState == StateType.CHASE)
        {
            Debug.Log("calculate 3");
            m_agent.Warp(transform.position);
            m_pathToPlayer = new NavMeshPath();
            m_agent.CalculatePath(m_player.position, m_pathToPlayer);
        }
    }

    void ChasePlayer()
    {
        if (m_pathToPlayer.corners.Length > 1 && !m_died && m_currentState != StateType.IDLE)
        {
            transform.LookAt(m_player);
            Debug.Log("Chase State");
           // m_currentState = 2;
            m_currentState = StateType.CHASE;

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
        if (!m_died && m_canAttack)
        {
            m_canAttack = false;
            m_rb.velocity = new Vector3(0, 0, 0);
            m_currentState = StateType.ATTACK;
            transform.LookAt(m_player);

            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1.0f, 1.0f, 1.0f), transform.rotation);
            foreach (var hitCollider in colliders)
            {
                //if its the player, then take damage
                //Make sure player object tag is set to "Player"
                if (hitCollider.CompareTag("Player"))
                {
                    Debug.Log("Attacking Player");

                    IHealth.Damage damageStruct = new IHealth.Damage();
                    damageStruct.damageAmount = m_damage;
                    damageStruct.type = IHealth.DamageType.ENEMY;

                    //Take Damage
                    IHealth health = m_player.GetComponent<IHealth>();
                    health.TakeDamage(damageStruct);
                    m_player.GetComponent<PlayerController>().PlayerKnockBack(gameObject);


                }
            }

            StartCoroutine(AttackCooldwon());
        }

    }

    public void ChangeState(StateType state)
    {
        switch(state)
        {
            case StateType.IDLE:
                Freeze();
                break;
            case StateType.CHASE:
                ChasePlayer();
                break;
            default:
                break;
        }
    }

    IEnumerator AttackCooldwon()
    {

        //  m_attackParticle.SetActive(true);
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(m_attack_cooldown);
        m_canAttack = true;
       // m_attackParticle.SetActive(false);
    }




    void death()
    {
        m_died = true;
        PersistentPrefs.GetInstance().m_currentSaveFile.SetFlag(gameObject.scene.name + "_EnemyKilled_" + gameObject.GetInstanceID(), true);
        m_deadEvent?.Invoke(gameObject);
        StartCoroutine(DestroyObject());
 

        m_rb.AddForce(-transform.forward * m_deathForce);
        m_rb.AddForce(transform.up * m_deathForce);
    }

    IEnumerator DestroyObject()
    {
        Instantiate(m_destroyParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);
        if (m_deathDrop != null)
            Instantiate(m_deathDrop, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    IEnumerator StopMoving()
    {
        m_agent.isStopped = true;
        m_currentState = StateType.IDLE;
        m_rb.velocity = new Vector3(0, 0, 0);
        yield return null;
       yield return new WaitForSeconds(1f);
  
        m_currentState = StateType.CHASE;
        m_takingDamage = false;
    }

    void Freeze()
    {
        m_agent.isStopped = true;
        m_currentState = StateType.IDLE;
        m_rb.velocity = new Vector3(0, 0, 0);
       
        m_currentState = StateType.IDLE;
        m_takingDamage = false;
    }

    public void Dodge()
    {
        if(m_currentState != StateType.IDLE)
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
        m_currentState = StateType.IDLE;
        m_takingDamage = true;
        m_rb.AddForce(-transform.forward * m_pushBackForce);
        StartCoroutine(StopMoving());

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
