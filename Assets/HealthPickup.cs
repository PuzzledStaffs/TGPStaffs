using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int HeartsToGive;
    public GameObject CollectParticle;
    public AudioClip CollectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().AddHealth(HeartsToGive);
            GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Animator>().SetTrigger("HealthGained");
            Instantiate(CollectParticle, transform.position, Quaternion.identity);
            other.GetComponent<AudioSource>().PlayOneShot(CollectSound);
            Destroy(this.gameObject);
        }
    }
}
