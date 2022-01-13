using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
   public enum StateType  
    {
        IDLE,
        ROAM,
        ATTACK,
    };

    protected StateType type;

}
