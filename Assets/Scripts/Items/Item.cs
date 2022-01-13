using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Weapons/item")]
public class Item : ScriptableObject
{
    new public string name = "Item Name";
    public Sprite ItemIcon = null;
    public float ItemDamage = 0.0f;
    public int CombatType = 0;
    public float Cooldown = 0.5f;
    public AudioClip ItemSound;


    public virtual void ItemActionRightClick()
    {
        switch (CombatType)
        {
            case 0: //Sword
                Debug.Log("Sword Right Click Attack");
                break;
            case 1: //Bow
                Debug.Log("Bow Right Click Attack");
                break;
        }
    }

    public virtual void ItemActionLeftClick()
    {
        switch (CombatType)
        {
            case 0: //Sword
                Debug.Log("Sword Left Click Attack");
                break;
            case 1: //Bow
                Debug.Log("Bow Left Click Attack");
                break;
        }
    }
}
