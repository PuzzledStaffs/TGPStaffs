using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonInfo : MonoBehaviour
{
    public Item WheelItem;
    public Image WheelIcon;


    private void Start()
    {
        WheelIcon.sprite = WheelItem.ItemIcon;
    }

}
