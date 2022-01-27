using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    public Item ItemToGive;
    //public UnityEvent m_pickedUp;
    public GameObject CollectParticle;
    public AudioClip CollectionSound;
    public int ItemID;

    void Start()
    {
        PersistentPrefs prefs = FindObjectOfType<PersistentPrefs>();

        switch (ItemID)
        {
            case 1:
                if (prefs.m_item1Unlocked) { Destroy(gameObject); }
                break;
            case 2:
                if (prefs.m_item2Unlocked) { Destroy(gameObject); }
                break;
            case 3:
                if (prefs.m_item3Unlocked) { Destroy(gameObject); }
                break;
            case 4:
                if (prefs.m_item4Unlocked) { Destroy(gameObject); }
                break;
            case 5:
                if (prefs.m_item5Unlocked) { Destroy(gameObject); }
                break;
            case 6:
                if (prefs.m_item6Unlocked) { Destroy(gameObject); }
                break;
            case 7:
                if (prefs.m_item7Unlocked) { Destroy(gameObject); }
                break;
            case 8:
                if (prefs.m_item8Unlocked) { Destroy(gameObject); }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player")
        {
            Debug.Log("Collision with player");
            WeaponWheelController WeaponWheel = other.GetComponent<PlayerController>().m_weaponWheelController;

            int i = 0;
            foreach (WeaponButtonInfo button in WeaponWheel.Buttons)
            {
                i++;
                if (button.WheelItem == ItemToGive)
                {
                    button.ItemBlocked = false;
                    FindObjectOfType<PersistentPrefs>().UnlockItem(i);
                    break;
                }
            }
            other.GetComponent<AudioSource>().PlayOneShot(CollectionSound);
            WeaponWheel.ItemUnlockedUI.ItemUnlocked();
            //m_pickedUp?.Invoke();
            Instantiate(CollectParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
