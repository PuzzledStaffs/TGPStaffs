using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "Item Name";
    public Sprite ItemIcon = null;
    public float ItemDamage = 0.0f;
    public int CombatType = 0;
    public float Cooldown = 0.5f;
    public AudioClip ItemSound;
    public bool CanUse = true;


    public virtual void LeftClickAction()
    {

    }

    public virtual void RightClickAction()
    {

    } 
}
