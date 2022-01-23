using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : Trap
{

    public override void EnterRoomEnabled()
    {

    }
    public override void ExitRoomDisabled()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerController>().Respawn();
            other.transform.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}
