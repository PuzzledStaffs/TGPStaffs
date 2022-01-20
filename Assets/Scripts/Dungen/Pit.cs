using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerController>().Respawn();
            other.transform.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}
