using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Enemy e;
    public IdleState idle;
    public AttackState attack;
    public RoamingState roam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(State.StateType type)
    {
        switch (type)
        {
            case State.StateType.IDLE:
                roam.enabled = false;
                attack.enabled = false;
                idle.enabled = true;
                break;
            case State.StateType.ROAM:
                roam.enabled = true;
                attack.enabled = false;
                idle.enabled = false;
                break;
            case State.StateType.ATTACK:
                roam.enabled = false;
                attack.enabled = true;
                idle.enabled = false;
                break;
        }
    }
}
