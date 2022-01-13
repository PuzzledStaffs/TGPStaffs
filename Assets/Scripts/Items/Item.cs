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

    public virtual void LeftClickAction()
    {

    }

    public virtual void RightClickAction()
    {

    }


}
