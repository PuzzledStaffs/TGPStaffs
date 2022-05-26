using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int HeartsToGive;
    public GameObject CollectParticle;
    public AudioClip CollectSound;

    private void Start()
    {
        if (PersistentPrefs.GetInstance().m_currentSaveFile.HasFlag(gameObject.scene.name + "_PickedUp_" + gameObject.transform.parent.parent.name + "_" + gameObject.name))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().AddHealth(HeartsToGive);
            //GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Animator>().SetTrigger("HealthGained");
            Instantiate(CollectParticle, transform.position, Quaternion.identity);
            other.GetComponent<AudioSource>().PlayOneShot(CollectSound);
            PersistentPrefs.GetInstance().m_currentSaveFile.AddFlag(gameObject.scene.name + "_PickedUp_" + gameObject.transform.parent.parent.name + "_" + gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
