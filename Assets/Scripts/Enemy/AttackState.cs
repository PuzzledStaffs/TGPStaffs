using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AttackState : State
{
    public StateManager manager;
    protected NavMeshAgent agent;
    public GameObject player;
    public float cooldown;
    public float maxCooldown;  
    public float animationTime;
    public int damage;
    public float distance;   
    

    // Start is called before the first frame update
   public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //slows enemy down to stop from overshooting
        agent.autoBraking = true;

        cooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //cooldown start
        cooldown -= Time.deltaTime;

        //Go to the player
        agent.SetDestination(player.transform.position);

        //Calculate distance
        distance = Vector3.Distance(transform.position, player.transform.position);


        //If enemy is too close, it will stop to attack
        if (distance < 3.0f)
        {
            agent.isStopped = true;
        }
        else 
        {
            agent.isStopped = false;
        }


        //if the cooldown has finished, attack
        if (cooldown <= 0) 
        {
            StartCoroutine(OnAttack());
            cooldown = maxCooldown;
        }

        //If the AI can't see the player, stop attacking
        if (!manager.e.fieldOfView.inFOV)
        {
            manager.ChangeState(StateType.IDLE);
        }

    }

    IEnumerator OnAttack()
    {
        //TODO: Play animation
        yield return new WaitForSeconds(animationTime / 2);

        //If an object is nearby, check if its the player
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1.0f, 1.0f, 1.0f), transform.rotation);
        foreach (var hitCollider in colliders)
        {
            //if its the player, then take damage
            //Make sure player object tag is set to "Player"
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("hit");

                //Take Damage
                IHealth health = player.GetComponent<IHealth>();
                health.TakeDamage(damage);
                
            }
        }
    }
}
