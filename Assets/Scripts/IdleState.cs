using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public float timer;
    float maxTime;
    public StateManager stateManager;
    public bool isAlive = true;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        maxTime = timer;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.isStopped = true;

        if (!isAlive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            agent.isStopped = false;
            timer = maxTime;
      
            stateManager.ChangeState(StateType.ROAM);
        }
       // stateManager.ChangeState(StateType.ROAM);
    }


}

