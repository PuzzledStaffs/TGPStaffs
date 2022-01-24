using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item ItemToGive;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Collision with player");
            WeaponWheelController WeaponWheel = other.GetComponent<PlayerController>().m_weaponWheelController;

            foreach(WeaponButtonInfo button in WeaponWheel.Buttons)
            {
                if(button.WheelItem == ItemToGive)
                {
                    button.ItemBlocked = false;
                }
            }
            WeaponWheel.ItemUnlockedUI.ItemUnlocked();
            Destroy(this.gameObject);
        }
    }

}
