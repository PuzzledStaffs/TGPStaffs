using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disablePond : MonoBehaviour
{
    public GameObject pond;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pond.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pond.SetActive(true);
        }
    }
}
