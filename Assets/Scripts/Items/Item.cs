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


    public virtual void ItemActionRightClick(int Type)
    {
        switch (Type)
        {
            case 0: //Sword
                Debug.Log("Sword Right Click Attack");
                break;
            case 1: //Bow
                Debug.Log("Bow Right Click Attack");
                break;
        }
    }

    public virtual void ItemActionLeftClick(int Type)
    {
        switch (Type)
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
