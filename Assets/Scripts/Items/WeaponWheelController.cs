using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponWheelController : MonoBehaviour
{
    public GameObject WeaponWheel;
    public TextMeshProUGUI WeaponSelectedText;
    public Item CurrentItem;


    public void ToggleWheel()
    {
        if (WeaponWheel.activeSelf)
        {
            WeaponWheel.SetActive(false);
        }
        else
        {
            WeaponWheel.SetActive(true);
        }
    }

    private void Update()
    {
        // this is temporary for testing - cause i didnt want to set up the input stuff properly yet
        var keyboard = Keyboard.current;
        if (keyboard.tabKey.wasPressedThisFrame)
        {
            ToggleWheel();
        }

        if (keyboard.wKey.wasPressedThisFrame)
        {
            CurrentItem.LeftClickAction();
        }



    }

    public void UpdateText(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        WeaponSelectedText.text = itemSelected.name;
    }

    public void SelectItem(WeaponButtonInfo weaponScript)
    {
        Item itemSelected = weaponScript.WheelItem;
        CurrentItem = itemSelected;
        ToggleWheel();
        Debug.Log("Item Selected: " + itemSelected.name);
    }
}
