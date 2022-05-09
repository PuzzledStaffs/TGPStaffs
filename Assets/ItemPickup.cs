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
        switch (ItemID)
        {
            case 1:
                if (PersistentPrefs.m_currentSaveFile.item1Unlocked) { Destroy(gameObject); }
                break;
            case 2:
                if (PersistentPrefs.m_currentSaveFile.item2Unlocked) { Destroy(gameObject); }
                break;
            case 3:
                if (PersistentPrefs.m_currentSaveFile.item3Unlocked) { Destroy(gameObject); }
                break;
            case 4:
                if (PersistentPrefs.m_currentSaveFile.item4Unlocked) { Destroy(gameObject); }
                break;
            case 5:
                if (PersistentPrefs.m_currentSaveFile.item5Unlocked) { Destroy(gameObject); }
                break;
            case 6:
                if (PersistentPrefs.m_currentSaveFile.item6Unlocked) { Destroy(gameObject); }
                break;
            case 7:
                if (PersistentPrefs.m_currentSaveFile.item7Unlocked) { Destroy(gameObject); }
                break;
            case 8:
                if (PersistentPrefs.m_currentSaveFile.item8Unlocked) { Destroy(gameObject); }
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
                    PersistentPrefs.m_currentSaveFile.UnlockItem(i);
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
