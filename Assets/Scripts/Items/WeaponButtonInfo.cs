using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonInfo : MonoBehaviour
{
    public Item WheelItem;
    public Image WheelIcon;
    public bool ItemBlocked;


    private void Start()
    {
        WheelIcon.sprite = WheelItem.ItemIcon;
    }

    public void Update()
    {
        GetComponent<Button>().interactable = ItemBlocked;
        WheelIcon.color = new Color(0,0,0,0.3f);
    }

}


