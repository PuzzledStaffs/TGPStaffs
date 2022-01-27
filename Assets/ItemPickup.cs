using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    public Item ItemToGive;
    public UnityEvent m_pickedUp;
    public GameObject CollectParticle;
    public AudioClip CollectionSound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Collision with player");
            WeaponWheelController WeaponWheel = other.GetComponent<PlayerController>().m_weaponWheelController;

            int i = 0;
            foreach(WeaponButtonInfo button in WeaponWheel.Buttons)
            {
                i++;
                if(button.WheelItem == ItemToGive)
                {
                    button.ItemBlocked = false;
                    FindObjectOfType<PersistentPrefs>().UnlockItem(i);
                    break;
                }
            }
            other.GetComponent<AudioSource>().PlayOneShot(CollectionSound);
            WeaponWheel.ItemUnlockedUI.ItemUnlocked();
            m_pickedUp?.Invoke();
            Instantiate(CollectParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
