using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bow Item")]
public class BowItem : Item
{
    public override void LeftClickAction(PlayerController pc)
    {
       // base.LeftClickAction();
        Debug.Log("BOW FIRE LEFT!");
    }


    public override void RightClickAction()
    {
        base.RightClickAction();
        Debug.Log("BOW FIRE RIGHT!");
    }
}

