using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fieldOfView.inFOV)
        {
            manager.ChangeState(State.StateType.ATTACK);
        }
    }

}