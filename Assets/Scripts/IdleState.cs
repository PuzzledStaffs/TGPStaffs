using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public int timer;
    public StateManager stateManager;
    public bool isAlive = true;
    StateType idlestate = StateType.IDLE;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        while (isAlive)
        {
            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(timer);
        this.type = StateType.ROAM;
        stateManager.ChangeState(type);
    }
}

