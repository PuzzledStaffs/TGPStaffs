using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWall : Trap
{

    [SerializeField] AnimationCurve m_wallTravelOut;
    [SerializeField] Vector3 m_StartLocation;
    [SerializeField] Vector3 m_EndLocation;
    private float inter = 0;


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
        inter += 0.1f;
    }
}
