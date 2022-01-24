using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public float timer;
    float maxTime;
    public StateManager stateManager;
    public bool isIdle = true;
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
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!isIdle)
            {
                agent.isStopped = false;
                timer = maxTime;

                stateManager.ChangeState(StateType.ROAM);
            }
            timer = maxTime;
        }
    }


}

