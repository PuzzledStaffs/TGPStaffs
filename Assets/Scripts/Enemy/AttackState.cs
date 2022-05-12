using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[System.Serializable]
public class AttackState : State
{
    [FormerlySerializedAs ("manager")]
    public StateManager m_manager;
    [FormerlySerializedAs("agent")]
    protected NavMeshAgent m_agent;
    [FormerlySerializedAs("player")]
    public GameObject m_player;
    [FormerlySerializedAs("cooldown")]
    public float m_cooldown;
    [FormerlySerializedAs("maxCooldown")]
    public float m_maxCooldown;
    [FormerlySerializedAs("animationTime")]
    public float m_animationTime;
    [FormerlySerializedAs("damage")]
    public int m_damage;
    [FormerlySerializedAs("distance")]
    public float m_distance;
    [FormerlySerializedAs("animator")]
    public Animator m_animator;
   // bool m_HasAttacked = false;


    // Start is called before the first frame update
    public virtual void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        //slows enemy down to stop from overshooting
        m_agent.autoBraking = true;

        m_cooldown = 0;
    }

    public  void OnEnable()
    {
        if(m_player == null)
            m_player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        //cooldown start
        m_cooldown -= Time.deltaTime;

        //Go to the player
        m_agent.SetDestination(m_player.transform.position);

        //Calculate distance
        m_distance = Vector3.Distance(transform.position, m_player.transform.position);


        //If enemy is too close, it will stop to attack
        if (m_distance < 2.0f)
        {
            m_agent.isStopped = true;
        }
        else 
        {
            m_agent.isStopped = false;
        }


        //if the cooldown has finished, attack
        if (m_cooldown <= 0) 
        {
            
            StartCoroutine(OnAttack());
            m_cooldown = m_maxCooldown;
            // m_HasAttacked = true;
        }

        //If the AI can't see the player, stop attacking
        if (!m_manager.m_enemy.m_fieldOfView.m_inFOV)
        {
            m_manager.ChangeState(StateType.IDLE);
        }

    }

    IEnumerator OnAttack()
    {
        m_animator.ResetTrigger("Attack");
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(m_animationTime / 2);

       

        //If an object is nearby, check if its the player
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
                m_player.GetComponent<PlayerController>().ApplyKnockack(transform.position);


            }
        }
      
        //m_HasAttacked = false;
    }
}
