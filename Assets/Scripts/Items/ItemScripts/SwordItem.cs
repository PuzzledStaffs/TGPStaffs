using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Sword Item")]
public class SwordItem : Item
{


    public override void LeftClickAction()
    {
        Debug.Log("SWORD SLASH LEFT");
    }


    public override void RightClickAction()
    {
        Debug.Log("SWORD SLASH RIGHT");
    }
}
