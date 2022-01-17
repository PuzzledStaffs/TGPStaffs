using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingState : State
{
    public Transform[] points;
    int destinationPoint = 0;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //for continous movement
        agent.autoBraking = false;
        agent.isStopped = false;
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        //Set desitination to the next point just before getting to current point
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        //checks if there are any points.
        if (points.Length == 0)
        {
            Debug.Log("No points!");
            return;
        }

        //Set the next point as the destination
        agent.SetDestination(points[destinationPoint].position);

        //Update the next point, cycles if it reaches the end
        destinationPoint = (destinationPoint + 1) % points.Length;

    }
}
