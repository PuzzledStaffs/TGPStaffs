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
            GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Animator>().SetTrigger("HealthGained");
            other.GetComponent<PlayerController>().SetHealth(HeartsToGive);
            Instantiate(CollectParticle, transform.position, Quaternion.identity);
            other.GetComponent<AudioSource>().PlayOneShot(CollectSound);
            Destroy(this.gameObject);
        }
    }
}
