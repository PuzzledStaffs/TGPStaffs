using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class State : MonoBehaviour
{
   public enum StateType  
    {
        IDLE,
        ROAM,
        CHASE,
        ATTACK,
    };

    [FormerlySerializedAs("type")]
    protected StateType m_type;

}
