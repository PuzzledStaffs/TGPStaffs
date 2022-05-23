
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
    protected Transform m_player;
    protected NavMeshPath m_pathToPlayer;
    public Rigidbody m_rb;
    public float m_pushBackForce;
    public float m_deathForce;
    public GameObject m_destroyParticle;
    public Animator m_animator;
    protected bool m_canAttack = true; //cooldown
                                       // public Vector3 m_offset; unused?
    public float m_dodgeChance = 0;
    protected bool m_takingDamage = false;
    public System.Action<GameObject> m_deadEvent;
    [FormerlySerializedAs("m_DeathDrop")]
    [SerializeField] GameObject m_deathDrop;

    [FormerlySerializedAs("m_stateType")]
    public StateType m_currentState;
    public int m_damage;

    public float m_attack_cooldown;
    public LayerMask m_whatIsPlayer, m_whatIsGround;

    public bool m_died = false;

    public RectTransform m_healthBarCanvas;
    public RectTransform m_healthBarMask;
    [Header("---------------States------------------------------------------------------------")]
    public float m_sightRange, m_attackRange;
    public bool m_playerInSightRange, m_playerInAttackRange;

    // public GameObject m_attackParticle; unused

    protected virtual void Start()
    {
       
    }

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updatePosition = false;
        m_agent.updateRotation = false;

        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_EnemyKilled_" + gameObject.scene.name + "_" + gameObject.transform.parent.parent.name + "_" + gameObject.name))
        {
            m_died = true;
            gameObject.SetActive(false);
            return;
        }

        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_currentState = StateType.IDLE;
        m_pathToPlayer = new NavMeshPath();
        InvokeRepeating("CalculatePath", 0.5f, 0.5f);
    }

    protected virtual void Update()
    {
        m_healthBarCanvas.position = transform.position + Vector3.forward;
        Vector3 lookPos = Camera.main.transform.position - m_healthBarCanvas.position;
        lookPos.x = 0;
        lookPos.z = 0;
        m_healthBarCanvas.rotation = Quaternion.LookRotation(lookPos);

        ChangeState(m_currentState);
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
        if (m_currentState == StateType.CHASE)
        {
            m_agent.Warp(transform.position);
            //m_pathToPlayer = new NavMeshPath();

            m_agent.CalculatePath(m_player.position, m_pathToPlayer);
            Debug.Log(m_agent.CalculatePath(m_player.position, m_pathToPlayer));
        }
    }

    protected virtual void ChasePlayer()
    {
        Debug.Log("Calculate Path1");
        CalculatePath();
        Debug.Log(m_pathToPlayer.corners.Length);
        if (m_pathToPlayer.corners.Length > 1 && !m_died && m_currentState != StateType.IDLE)
        {
            transform.LookAt(m_player);
            Debug.Log("Calculate Path2");

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

    public virtual void AttackPlayer()
    {

    }

    protected IEnumerator Wait()
    {
        m_agent.isStopped = true;
        m_rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(1f);


        m_takingDamage = false;
    }

    public virtual void ChangeState(StateType state)
    {
        switch (state)
        {
            case StateType.IDLE:
                m_currentState = StateType.IDLE;
                IdleState();
                break;
            case StateType.CHASE:
                m_currentState = StateType.CHASE;
                ChasePlayer();
                break;
            default:
                break;
        }
    }


    protected IEnumerator AttackCooldown()
    {
        //  m_attackParticle.SetActive(true);
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(m_attack_cooldown);
        m_canAttack = true;
        // m_attackParticle.SetActive(false);
    }


    virtual protected void IdleState()
    {
        m_agent.isStopped = true;
        m_rb.velocity = new Vector3(0, 0, 0);
        m_takingDamage = false;
    }


    //Not implemented yet
    public void Dodge()
    {
        if (m_currentState != StateType.IDLE)
        {
            m_dodgeChance += 1 * Time.deltaTime;
            float randomNum = Random.Range(0, 1000 - (m_dodgeChance * 200));
            if (randomNum <= 1)
            {
                Debug.Log("Dodged with a percent of: " + m_dodgeChance * 10);
                m_dodgeChance = 0;
                gameObject.GetComponent<Animator>().SetTrigger("Dodge");
                StartCoroutine(Wait());
            }
        }
    }


    #region Health

    void death()
    {
        m_died = true;
        PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_EnemyKilled_" + gameObject.scene.name + "_" + gameObject.transform.parent.parent.name + "_" + gameObject.name);
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
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return m_health;
    }

    public virtual void TakeDamage(IHealth.Damage damage)
    {
        if (damage.type == IHealth.DamageType.SWORD)
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
        m_healthBarMask.sizeDelta = new Vector2(4.5f * (m_health / 20.0f), 0.5f);
        m_animator.SetTrigger("EnemyHit");

        m_rb.AddForce(-transform.forward * m_pushBackForce);
        m_takingDamage = true;      
        StartCoroutine(Wait());

        if (IsDead())
        {
            death();
        }
    }


    public bool IsDead()
    {
        return m_health <= 0 || m_died;
    }

    #endregion
}


