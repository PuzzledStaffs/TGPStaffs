using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public StateManager manager;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        //if(true)
        this.type = StateType.IDLE;
        manager.ChangeState(type);

    }
}
