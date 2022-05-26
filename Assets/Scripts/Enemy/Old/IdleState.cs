using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class IdleState : State
{
    [FormerlySerializedAs("timer")]
    public float m_timer;
    [FormerlySerializedAs("maxTime")]
    float m_maxTime;
    [FormerlySerializedAs("stateManager")]
    public StateManager m_stateManager;
    [FormerlySerializedAs("isIdle")]
    public bool m_isIdle = true;
    [FormerlySerializedAs("agent")]
    NavMeshAgent m_agent;

    // Start is called before the first frame update
    void Start()
    {
        m_maxTime = m_timer;
        m_agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        m_agent.isStopped = true;
        m_timer -= Time.deltaTime;

        if (m_timer <= 0)
        {
            if (!m_isIdle)
            {
                m_agent.isStopped = false;
                m_timer = m_maxTime;

                m_stateManager.ChangeState(StateType.ROAM);
            }
            m_timer = m_maxTime;
        }
    }


}

