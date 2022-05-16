using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "Item Name";
    public Sprite ItemIcon = null;
    public int ItemDamage = 0;
    public int CombatType = 0;
    public float MaxCooldown = 0.5f;
    public AudioClip ItemSound;
    public bool CanUse = true;
    public bool ItemHold;
    public string TypeName;

    public virtual void LeftClickAction(PlayerController pc)
    {

    }
    public virtual void ReleaseAction(PlayerController pc)
    {

    }
}
