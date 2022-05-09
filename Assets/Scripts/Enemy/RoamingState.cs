using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//#DEFINE MAX_VELOCITY 100

public class RoamingState : State
{
    public Transform[] m_points;
    int m_destinationPoint = 0;
    NavMeshAgent m_agent;
    Vector3 m_center;
    Vector3 m_velocity;

    //https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
    //https://forum.unity.com/threads/wandering-around.76687/
    //https://gamedev.stackexchange.com/questions/106737/wander-steering-behaviour-in-3d


    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        //    //for continous movement
        //    m_agent.autoBraking = false;
        //    m_agent.isStopped = false;
        //    GotoNextPoint();
    }

    //// Update is called once per frame
    void Update()
    {
        //Set desitination to the next point just before getting to current point
        if (m_agent.remainingDistance < 0.5f)
        {
            Wander();
        }
    }

    //void GotoNextPoint()
    //{
    //    //checks if there are any points.
    //    if (m_points.Length == 0)
    //    {
    //        Debug.Log("No points!");
    //        return;
    //    }

    //    //Set the next point as the destination
    //    m_agent.SetDestination(m_points[m_destinationPoint].position);

    //    //Update the next point, cycles if it reaches the end
    //    m_destinationPoint = (m_destinationPoint + 1) % m_points.Length;

    //}

    Vector3 Wander()
    {
        Vector3 circleCenter;
        circleCenter = m_agent.velocity;
        circleCenter.Normalize();
        circleCenter *= 10;

        Vector3 displacement = new Vector3(0, -1, 0);
        displacement *= 10;

        displacement.x = Mathf.Cos(45) * displacement.magnitude;
        displacement.z = Mathf.Sin(45) * displacement.magnitude;

        float angle = 45;
        angle += Random.Range(0, 360) * 20 - 10;

        Vector3 wanderForce;
        wanderForce = circleCenter + displacement;

        Vector3 steering = wanderForce;
        steering = steering / 20;
        m_agent.velocity = m_agent.velocity + steering;
        m_agent.SetDestination(circleCenter);


        return circleCenter;
    }
}


