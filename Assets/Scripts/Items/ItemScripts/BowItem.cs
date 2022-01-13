using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bow Item")]
public class BowItem : Item
{
    public override void LeftClickAction()
    {
        Debug.Log("BOW FIRE LEFT");
    }


    public override void RightClickAction()
    {
        Debug.Log("BOW FIRE RIGHT");
    }
}

