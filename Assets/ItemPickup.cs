using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ItemPickup : MonoBehaviour
{
    [FormerlySerializedAs("ItemToGive")]
    public Item m_itemToGive;
    [FormerlySerializedAs("CollectParticle")]
    public GameObject m_collectParticle;
    [FormerlySerializedAs("CollectionSound")]
    public AudioClip m_collectionSound;
    [FormerlySerializedAs("ItemID")]
    public int m_itemID;

    void Start()
    {
        switch (m_itemID)
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
            foreach (WeaponButtonInfo button in WeaponWheel.m_buttons)
            {
                i++;
                if (button.WheelItem == m_itemToGive)
                {
                    button.ItemBlocked = false;
                    PersistentPrefs.m_currentSaveFile.UnlockItem(i);
                    break;
                }
            }
            other.GetComponent<AudioSource>().PlayOneShot(m_collectionSound);
            WeaponWheel.m_itemUnlockedUI.ItemUnlocked();
            //m_pickedUp?.Invoke();
            Instantiate(m_collectParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
