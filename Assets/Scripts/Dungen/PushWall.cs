using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PushWall : Trap
{

    [SerializeField] AnimationCurve m_wallTravelOut;
    [FormerlySerializedAs("m_StartLocation")]
    [SerializeField] Vector3 m_startLocation;
    [FormerlySerializedAs("m_EndLocation")]
    [SerializeField] Vector3 m_endLocation;
    [FormerlySerializedAs("inter")]
    private float m_inter = 0;


    public override void EnterRoomEnabled()
    {
        //Begin push pull sequence  
    }

    public override void ExitRoomDisabled()
    {
        //End push pull sequence

        //Reset position
    }

    private IEnumerator PushWallAction()
    {        
        yield return new WaitForFixedUpdate();
        m_inter += 0.1f;
    }
}
