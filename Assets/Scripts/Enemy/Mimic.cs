using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : EnemyController , IAltInteractable
{
    bool m_isActive = false;
    [SerializeField] List<GameObject> m_ActiveChest;
    [SerializeField] List<GameObject> m_InactiveChest;


    public void AltInteract()
    {
        m_isActive = true;
        ChangeState(StateType.CHASE);
        SetActiveApperance(true);
        m_rb.isKinematic = false;
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;



    }

    protected override void Start()
    {
        base.Start();
        m_rb.isKinematic = true;        
        m_rb.constraints = RigidbodyConstraints.FreezeAll;
        SetActiveApperance(false);

    }

    void SetActiveApperance(bool active)
    {
        foreach (GameObject part in m_ActiveChest)
        {
            part.SetActive(active);
        }
        foreach (GameObject part in m_InactiveChest)
        {
            part.SetActive(!active);
        }
    }

    protected override void ChasePlayer()
    {
        if(m_isActive)
            base.ChasePlayer();
    }

    override public void AttackPlayer()
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

            StartCoroutine(AttackCooldown());
        }
    }

    public InteractInfo CanInteract()
    {
        return new InteractInfo(!m_isActive, "OpenChest", 2);
    }

    override public void ChangeState(StateType state)
    {
        switch (state)
        {
            case StateType.IDLE:
                m_currentState = StateType.IDLE;
                Wait();
                break;
            case StateType.CHASE:
                if (m_isActive)
                {
                    m_currentState = StateType.CHASE;
                    ChasePlayer();
                }
                break;
            default:
                break;
        }
    }

    public override void TakeDamage(IHealth.Damage damage)
    {
        if(m_isActive)
            base.TakeDamage(damage);
    }
}
